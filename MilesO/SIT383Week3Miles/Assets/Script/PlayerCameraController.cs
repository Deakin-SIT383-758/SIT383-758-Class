using Fusion;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    public Camera playerCamera;

    public override void Spawned()
    {
        // Only enable the camera for the player who owns this object
        if (Object.HasInputAuthority)
        {
            playerCamera.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
        }
    }
}