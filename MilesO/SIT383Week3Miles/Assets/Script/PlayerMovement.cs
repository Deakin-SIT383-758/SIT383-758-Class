using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController ch;
    public float playerSpeed = 5f;

    public override void FixedUpdateNetwork()
    {

        if (!Object.HasInputAuthority)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * playerSpeed * Runner.DeltaTime;

        ch.Move(movement);

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E");
            Debug.Log("MAIN CAMERA: " + Camera.main);
            Debug.Log("MAIN CAMERA ENABLED: " + Camera.main.enabled);
            Debug.Log("MAIN CAMERA POSITION: " + Camera.main.transform.position);
            Debug.Log("MAIN CAMERA FORWARD: " + Camera.main.transform.forward);

            if (Camera.main == null)
            {
                Debug.Log("No main camera!");
                return;
            }

            if (Physics.Raycast(Camera.main.transform.position,
                                Camera.main.transform.forward,
                                out RaycastHit hit, 20f))
            {
                Debug.Log("Raycast hit: " + hit.collider.name);

                if (hit.collider.TryGetComponent(out SharedCube cube))
                {
                    Debug.Log("Found SharedCube script, calling RPC");
                    cube.RPC_ChangeColor();
                }
                else
                {
                    Debug.Log("Hit object has NO SharedCube script");
                }
            }
            else
            {
                Debug.Log("Raycast hit nothing");
            }

        }

    }
}