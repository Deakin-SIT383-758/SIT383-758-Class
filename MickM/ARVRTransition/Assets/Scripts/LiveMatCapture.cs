using Meta.XR;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using TMPro;

public class LiveMatCapture : MonoBehaviour
{
    public PassthroughCameraAccess cameraAccess;
    public MeshRenderer matRenderer;
    private Material _warpMaterial;
    private bool _isAccessAvailable = false;

    void Start()
    {
        _warpMaterial = matRenderer.material;
        leftWristText.text = "Startup. No access available";
    }

    public TextMeshPro leftWristText;
    void Update()
    {
        if (!_isAccessAvailable)
        {
            Texture cameraTex = cameraAccess.GetTexture();
            if (cameraTex != null)
            {
                _warpMaterial.SetTexture("_MainTex", cameraTex);
                _isAccessAvailable = true;
                Debug.Log("Camera Access Active!");

                leftWristText.text = "Camera Access Active";
            }
            return;
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            FreezeTexture();
        }
    }

    void FreezeTexture()
    {
        Texture currentFrame = cameraAccess.GetTexture();
        Texture2D staticCopy = new Texture2D(currentFrame.width, currentFrame.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(currentFrame, staticCopy);

        _warpMaterial.SetTexture("_BaseMap", staticCopy);
        //this.enabled = false;
        
        leftWristText.text = $"Diorama Floor Frozen. texture size: {currentFrame.width} x {currentFrame.height}";
    }
}