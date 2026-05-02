using Unity.Mathematics;
using UnityEngine;

public class TurnScript : MonoBehaviour
{
    public float TurnSpeed;
    InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new InputSystem_Actions();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float h = controls.Player.Move.ReadValue<Vector2>().x;
        transform.rotation *= Quaternion.AngleAxis(h * TurnSpeed * Time.deltaTime, Vector3.up);
    }
}
