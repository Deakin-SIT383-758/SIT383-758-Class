using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController ch;
    public float playerSpeed = 5f;
    public float gravity = -9.81f;
    private float yVelocity;

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)
            return;

        if (ch.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

        if (Input.GetKey(KeyCode.Space) && ch.isGrounded)
        {
            yVelocity = Mathf.Sqrt(-0.3f * gravity);
        }

        yVelocity += gravity * Runner.DeltaTime;
        movement.y = yVelocity;

        ch.Move(movement * playerSpeed * Runner.DeltaTime);

        Vector3 flatMovement = new Vector3(movement.x, 0, movement.z);
        if (flatMovement != Vector3.zero)
        {
            transform.forward = flatMovement;
        }
    }
}