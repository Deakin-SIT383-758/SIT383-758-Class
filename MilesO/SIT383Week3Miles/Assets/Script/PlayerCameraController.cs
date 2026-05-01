using Fusion;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    public Camera playerCamera;

    public override void Spawned()
    {
        // Enable camera only for the local player
        Camera cam = GetComponentInChildren<Camera>();

        if (Object.HasInputAuthority)
            cam.enabled = true;
        else
            cam.enabled = false;
    }

}