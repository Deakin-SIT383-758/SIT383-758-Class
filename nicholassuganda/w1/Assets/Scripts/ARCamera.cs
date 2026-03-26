using UnityEngine;
using TMPro;

public class ARCamera : MonoBehaviour
{
    public Material cubeMaterial;
    private WebCamTexture wcTexture;

    public TextMeshProUGUI statusText;
    public TextMeshProUGUI cameraText;
    public TextMeshProUGUI photoText;

    private WebCamDevice[] devices;
    private int currentCameraIndex = 0;

    void Start()
    {
        devices = WebCamTexture.devices;
        StartCamera();
    }

    void Update()
    {

    }

    void StartCamera()
    {
        if (wcTexture != null)
        wcTexture.Stop();

        wcTexture = new WebCamTexture(devices[currentCameraIndex].name);
        cubeMaterial.mainTexture = wcTexture;
        wcTexture.Play();

        cameraText.text = "Using: " + devices[currentCameraIndex].name;

        if (WebCamTexture.devices.Length == 0)
        {
            statusText.text = "No camera found!";
        } else
        {
            statusText.text = "Good";
        }
    }
    
    public void TakePhoto()
    {
        string fileName = "photo_" + System.DateTime.Now.Ticks + ".png";
        ScreenCapture.CaptureScreenshot(fileName);

        photoText.text = "Saved: " + fileName;
    }

    public void SwitchFrontCamera()
    {
        currentCameraIndex = (currentCameraIndex + 1) % devices.Length;
        StartCamera();
    }
    public void SwitchBackCamera()
    {
        currentCameraIndex = (currentCameraIndex - 1) % devices.Length;
        StartCamera();
    }
}
