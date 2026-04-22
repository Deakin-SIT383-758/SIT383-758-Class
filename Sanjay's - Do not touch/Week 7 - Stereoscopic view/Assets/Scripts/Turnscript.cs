using UnityEngine;

public class Turnscript : MonoBehaviour
{
    public float turnspeed = 100.0f;

    private InputSystem_Actions control;
    void Start()
    {
        control = new InputSystem_Actions();
        control.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float h = control.Player.Move.ReadValue<Vector2>().x;
        transform.rotation *= Quaternion.AngleAxis(h * turnspeed * Time.deltaTime, Vector3.up);
    }
}
