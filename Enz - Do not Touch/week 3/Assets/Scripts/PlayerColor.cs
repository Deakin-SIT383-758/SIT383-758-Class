using UnityEngine;
using Fusion;

public class PlayerColor : NetworkBehaviour
{
    [SerializeField] private Renderer playerRenderer;

    [Networked] private Vector3 NetworkColor { get; set; }

    public override void Spawned()
    {
        if (playerRenderer == null)
        {
            playerRenderer = GetComponent<Renderer>();
        }

        if (HasStateAuthority)
        {
            NetworkColor = new Vector3(Random.value, Random.value, Random.value);
        }

        ApplyColor();
    }

    public override void Render()
    {
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (playerRenderer == null) return;

        Color color = new Color(NetworkColor.x, NetworkColor.y, NetworkColor.z);
        playerRenderer.material.color = color;
    }
}