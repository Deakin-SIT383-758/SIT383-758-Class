using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Sensors : MonoBehaviour
{
    public TextMeshProUGUI gyroSense;
    public TextMeshProUGUI accelSense;
    public TextMeshProUGUI lightSense;

    UnityEngine.InputSystem.Gyroscope gyro;
    UnityEngine.InputSystem.Accelerometer accel;
    UnityEngine.InputSystem.LightSensor lights;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gyro = UnityEngine.InputSystem.Gyroscope.current;
        accel = UnityEngine.InputSystem.Accelerometer.current;
        lights = UnityEngine.InputSystem.LightSensor.current;

        InputSystem.EnableDevice(gyro);
        InputSystem.EnableDevice(accel);
        InputSystem.EnableDevice(lights);
    }

    // Update is called once per frame
    void Update()
    {
        if (gyro != null && gyro.enabled)
        {
            Vector3 angularVelocity = gyro.angularVelocity.ReadValue();
            gyroSense.text = "Gyroscope: " + angularVelocity;
        }
        if (accel != null && accel.enabled)
        {
            Vector3 acceleration = accel.acceleration.ReadValue();
            accelSense.text = "Accelerometer: " + acceleration;
        }
        if (lights != null && lights.enabled)
        {
            float lightLevel = lights.lightLevel.ReadValue();
            lightSense.text = "Light Sensor: " + lightLevel;
        }
    }
}
