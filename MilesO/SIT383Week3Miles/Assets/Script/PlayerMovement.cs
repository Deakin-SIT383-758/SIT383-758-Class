using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController ch;
    public float playerSpeed = 5f;

    [Header("Camera")]
    public Camera playerCamera;

    [Header("Visual")]
    public Renderer playerRenderer;

    [Header("Interaction")]
    public GameObject objectPrefab;

    //NETWORKED VARIABLE (synced across all clients)
    [Networked] public Color PlayerColor { get; set; }

    private float lastSpawnTime;

    public bool spawnPressed;

    public override void Spawned()
    {
        // Enable camera only for local player
        if (playerCamera != null)
            playerCamera.enabled = Object.HasInputAuthority;

        // Assign colour ONLY on server
        if (Object.HasStateAuthority)
        {
            PlayerColor = new Color(
                Random.value,
                Random.value,
                Random.value
            );
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out PlayerInputData input)) return;

        // ✅ Movement is now processed on BOTH client + server
        Vector3 movement = new Vector3(input.horizontal, 0, input.vertical)
                            * playerSpeed * Runner.DeltaTime;

        transform.position += movement;

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }

        // ✅ Only input authority triggers actions
        if (Object.HasInputAuthority && input.spawnPressed)
        {
            RPC_SpawnObject();
        }
    }

    public override void Render()
    {
        //Apply synced colour every frame
        if (playerRenderer != null)
        {
            playerRenderer.material.color = PlayerColor;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SpawnObject(RpcInfo info = default)
    {
        Vector3 spawnPos = transform.position + transform.forward + Vector3.up;

        Runner.Spawn(objectPrefab, spawnPos, Quaternion.identity);
    }
}