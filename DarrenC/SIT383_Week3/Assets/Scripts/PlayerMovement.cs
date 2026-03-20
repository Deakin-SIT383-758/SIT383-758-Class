using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController charCont;
    public float playerSpeed = 5.0f;

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * playerSpeed * Runner.DeltaTime;

        charCont.Move(movement);

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }
}
