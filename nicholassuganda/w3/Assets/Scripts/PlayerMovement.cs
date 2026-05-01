using UnityEngine;
using Fusion;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController ch;
    [SerializeField] private MeshRenderer capsuleRenderer;
    public float playerSpeed = 5f;

    [Networked] public string PlayerName { get; set; }
    [Networked] public Color PlayerColor { get; set; }
    [SerializeField] private TextMeshPro nameTag;

    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer = -1;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpRequested;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            PlayerName = $"Player {Object.InputAuthority.PlayerId}";
            PlayerColor = new Color(Random.value, Random.value, Random.value);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority == false)
        {
            return;
        }

        Vector3 feetPosition = transform.position + Vector3.down * (ch.height * 0.5f);
        isGrounded = Physics.CheckSphere(feetPosition, groundCheckRadius, groundLayer);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * playerSpeed * Runner.DeltaTime;
        ch.Move(movement);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }
        
        if (jumpRequested && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }

        velocity.y += gravity * Runner.DeltaTime;
        ch.Move(velocity * Runner.DeltaTime);

        if(movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }
    
    public override void Render()
    {
        if (nameTag != null)
        {
            nameTag.text = PlayerName;
            nameTag.color = PlayerColor;
        }

        if (capsuleRenderer != null)
        {
            capsuleRenderer.material.color = PlayerColor;
        }
    }
}