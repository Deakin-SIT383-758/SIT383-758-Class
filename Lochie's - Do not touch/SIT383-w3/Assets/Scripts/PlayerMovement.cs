using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController ch;
    public float moveSpeed;

    void Start()
    {
        moveSpeed = Random.Range(5,10);
    }

    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority == false)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Runner.DeltaTime;

        ch.Move(movement);

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }
}
