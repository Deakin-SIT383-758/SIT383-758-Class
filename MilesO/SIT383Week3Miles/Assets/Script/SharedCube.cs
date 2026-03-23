using Fusion;
using UnityEngine;

public class SharedCube : NetworkBehaviour
{
    [Networked] public Color CubeColor { get; set; }

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public override void Render()
    {
        if (rend != null)
            rend.sharedMaterial.color = CubeColor;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ChangeColor()
    {
        CubeColor = Random.ColorHSV();
    }
}