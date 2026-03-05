using UnityEngine;

public class PhysicalCameraTexture : MonoBehaviour
{
    public Material camTexMaterial;
    private WebCamTexture webcamTexture;

    void Start()
    {
        webcamTexture = new WebCamTexture();
        camTexMaterial.mainTexture = webcamTexture;
        webcamTexture.Play();
    }
}