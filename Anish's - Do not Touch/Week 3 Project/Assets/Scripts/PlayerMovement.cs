using UnityEngine;
using Fusion;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    public float playerSpeed = 5f;
    public float jumpHeight = 0.3f;
    public float gravity = -9.81f;

    private CharacterController ch;
    private float yVelocity;

    private void Awake()
    {
        ch = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input) == false)
            return;

        if (ch.isGrounded && yVelocity < 0f)
            yVelocity = -2f;

        Vector3 movement = new Vector3(
            input.movement.x,
            0f,
            input.movement.y
        );

        movement = Vector3.ClampMagnitude(movement, 1f);

        if (input.jump && ch.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        yVelocity += gravity * Runner.DeltaTime;

        Vector3 finalMove =
            movement * playerSpeed +
            Vector3.up * yVelocity;

        ch.Move(finalMove * Runner.DeltaTime);

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }
}