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
        if (!Object.HasInputAuthority) return;

        // Only process if we have valid network input
        if (GetInput(out PlayerInputData input))
        {
            Vector3 movement = new Vector3(input.horizontal, 0, input.vertical) * playerSpeed * Runner.DeltaTime;

            // Move using CharacterController
            ch.Move(movement);

            // Rotate toward movement direction
            if (movement != Vector3.zero)
            {
                transform.forward = movement;
            }

            // Spawn shared object
            if (input.spawn && Object.HasInputAuthority)
            {
                RPC_SpawnObject();
            }

            if (input.spawn && Runner.DeltaTime - lastSpawnTime > 0.5f)
            {
                lastSpawnTime = Runner.DeltaTime;
                RPC_SpawnObject();
            }

                Debug.Log("Input received");

            Debug.Log(Object.HasInputAuthority);
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
    void RPC_SpawnObject()
    {
        if (objectPrefab == null) return;

        Vector3 spawnPos = transform.position + transform.forward + Vector3.up;

        Runner.Spawn(objectPrefab, spawnPos, Quaternion.identity);
    }
}