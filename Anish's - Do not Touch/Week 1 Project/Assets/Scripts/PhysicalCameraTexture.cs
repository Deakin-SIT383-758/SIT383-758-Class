using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

using TMPro;

public class PhysicalCameraTexture : MonoBehaviour
{
    public Material camTexMaterial;
    private WebCamTexture webcamTexture;
    public TextMeshProUGUI outputText;
    private int currentCamera = 0;

    private void ShowCameras()
    {
        outputText.text = "";

        foreach (WebCamDevice d in WebCamTexture.devices)
        {
            outputText.text += d.name +
                (d.name == webcamTexture?.deviceName ? "*" : "") +
                "\n";
        }
    }

    public void NextCamera()
    {
        currentCamera =
            (currentCamera + 1) % WebCamTexture.devices.Length;

        webcamTexture.Stop();

        webcamTexture.deviceName =
            WebCamTexture.devices[currentCamera].name;

        webcamTexture.Play();

        ShowCameras();
    }

    void Update()
    {
        ShowCameras();

        if (webcamTexture == null)
        {
            webcamTexture = new WebCamTexture();

#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                webcamTexture = null;
            }
#endif
        }

        if (!webcamTexture.isPlaying)
        {
            camTexMaterial.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
    }
}