using UnityEngine;

public class TurnScript : MonoBehaviour
{
    public float turnspeed = 100.0f;
    private InputSystem_Actions contorls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        contorls = new InputSystem_Actions();
        contorls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float h = contorls.Player.Move.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.AngleAxis(h * turnspeed * Time.deltaTime, Vector3.up);
    }
}
