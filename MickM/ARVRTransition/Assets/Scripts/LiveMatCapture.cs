using Meta.XR;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using TMPro;

public class LiveMatCapture : MonoBehaviour
{
    public PassthroughCameraAccess cameraAccess;
    public MeshRenderer matRenderer;
    private Material warpMaterial;
    private bool cameraAccessAvailable = false;

    public OVRInput.Button setGroundTextureButton = OVRInput.Button.SecondaryIndexTrigger;
    public MeshRenderer debugRenderer;
    void Start()
    {
        warpMaterial = matRenderer.material;
        leftWristText.text = "Startup. No access available";
    }

    public TextMeshPro leftWristText;
    void Update()
    {
        if (!cameraAccessAvailable)
        {
            Texture cameraTex = cameraAccess.GetTexture();
            if (cameraTex != null)
            {
                warpMaterial.SetTexture("_MainTex", cameraTex);
                cameraAccessAvailable = true;
                Debug.Log("Camera Access Active!");

                leftWristText.text = "Camera Access Active";
            }
            return;
        }

        if (OVRInput.GetDown(setGroundTextureButton))
        {
            FreezeTexture();
        }
    }

    void FreezeTexture()
    {
        Texture currentFrame = cameraAccess.GetTexture();
        Texture2D staticCopy = new Texture2D(currentFrame.width, currentFrame.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(currentFrame, staticCopy);

        warpMaterial.SetTexture("_BaseMap", staticCopy);

        debugRenderer.material.SetTexture("_BaseMap", staticCopy);

        leftWristText.text = $"Diorama Floor Frozen. texture size: {currentFrame.width} x {currentFrame.height}";
    }
}