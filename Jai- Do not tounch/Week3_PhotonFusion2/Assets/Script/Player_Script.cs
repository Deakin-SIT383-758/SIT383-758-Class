using UnityEngine;
using Fusion;

public class Player_Script : NetworkBehaviour
{
    private float moveSpeed = 5.0f;
    void FixedUpdate()
    {
        // Get input from keyboard (WASD or Arrow Keys)
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 (left) to 1 (right)
        float verticalInput = Input.GetAxis("Vertical");   // -1 (back) to 1 (forward)

        // Calculate the movement direction relative to the object's orientation
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // Move the player using the transform component
        // Time.deltaTime ensures smooth movement regardless of frame rate
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 1. Try to get the NetworkObject component from the object hit
        if (collision.gameObject.TryGetComponent<NetworkObject>(out var networkObject))
        {

            // 2. Identify the player who owns/controls this object
            PlayerRef owner = networkObject.StateAuthority;

            // 3. Check if this owner is the Host
            // In Host Mode, the Runner.IsPlayer(owner) and comparing owner details works,
            // but specifically, the Host is the player who started as GameMode.Host.
            if (Runner.IsServer && owner == Runner.LocalPlayer)
            {
                Debug.Log("is host");
            }
            else
            {
                Destroy(this.gameObject);
            }

        }
    }
}
