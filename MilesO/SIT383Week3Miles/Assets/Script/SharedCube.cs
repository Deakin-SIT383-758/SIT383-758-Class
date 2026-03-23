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
        rend.material.color = CubeColor;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ChangeColor() // When player interacts with the cube, this RPC is called to change the color of the cube on the server
    {
        CubeColor = Random.ColorHSV();
    }
}