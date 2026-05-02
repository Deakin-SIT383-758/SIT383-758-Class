using UnityEngine;
using TMPro;

public class ARCamera : MonoBehaviour
{
    public Material cubeMaterial;
    public TextMeshProUGUI textBox;

    private WebCamTexture wcTexture;
    private WebCamDevice[] devices;
    private int currentCameraIndex = 0;

    void Start()
    {
        devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            StartCamera(currentCameraIndex);
        }
        else
        {
            textBox.text = "No camera found";
        }
    }

    void StartCamera(int cameraIndex)
    {
        if (wcTexture != null && wcTexture.isPlaying)
        {
            wcTexture.Stop();
        }

        wcTexture = new WebCamTexture(devices[cameraIndex].name);
        cubeMaterial.mainTexture = wcTexture;
        wcTexture.Play();

        textBox.text = "Camera active: " + devices[cameraIndex].name;
    }

    public void SwitchCamera()
    {
        if (devices.Length <= 1)
        {
            textBox.text = "Only one camera available";
            return;
        }

        currentCameraIndex++;

        if (currentCameraIndex >= devices.Length)
        {
            currentCameraIndex = 0;
        }

        StartCamera(currentCameraIndex);
    }
}