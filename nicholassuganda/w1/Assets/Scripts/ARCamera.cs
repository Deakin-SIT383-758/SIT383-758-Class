using UnityEngine;
using TMPro;

public class ARCamera : MonoBehaviour
{
    public Material cubeMaterial;
    private WebCamTexture wcTexture;

    public TextMeshProUGUI infoText;

    public float rotationSpeed = 5f;

    void Start()
    {
        // Enable gyroscope
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            infoText.text = "Gyroscope not supported";
        }

        // Start camera
        if (WebCamTexture.devices.Length == 0)
        {
            infoText.text = "No camera found";
            return;
        }

        wcTexture = new WebCamTexture();
        cubeMaterial.mainTexture = wcTexture;
        wcTexture.Play();

        infoText.text = "App started";
    }

    void Update()
    {
        // Get gyroscope rotation
        Vector3 rotation = Input.gyro.rotationRateUnbiased;

        // Rotate cube based on device movement
        transform.Rotate(rotation.x * rotationSpeed,
                         rotation.y * rotationSpeed,
                         rotation.z * rotationSpeed);

        // Show gyro values
        infoText.text = "Gyro:\n" +
                        "X: " + rotation.x.ToString("F2") + "\n" +
                        "Y: " + rotation.y.ToString("F2") + "\n" +
                        "Z: " + rotation.z.ToString("F2");
    }
}