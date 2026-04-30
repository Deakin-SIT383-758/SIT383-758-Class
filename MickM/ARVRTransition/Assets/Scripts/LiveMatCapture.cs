using Meta.XR;
using UnityEngine;

public class LiveMatCapture : MonoBehaviour
{
    public PassthroughCameraAccess cameraAccess;
    public MeshRenderer matRenderer;
    private Material warpMaterial;
    private bool cameraAccessAvailable = false;

    public OVRInput.Button setGroundTextureButton = OVRInput.Button.SecondaryIndexTrigger;
    public OVRInput.Button[] grabReleaseButtons = new OVRInput.Button[]
    {
        OVRInput.Button.PrimaryHandTrigger,
        OVRInput.Button.SecondaryHandTrigger
    };

    void Start()
    {
        warpMaterial = matRenderer.material;
    }

    void Update()
    {
        if (!cameraAccessAvailable)
        {
            Texture cameraTex = cameraAccess.GetTexture();
            if (cameraTex != null)
            {
                warpMaterial.SetTexture("_MainTex", cameraTex);
                cameraAccessAvailable = true;
            }
            return;
        }

        if (OVRInput.GetDown(setGroundTextureButton))
        {
            FreezeTexture();
        }
        else
        {
            for (int i = 0; i < grabReleaseButtons.Length; i++)
            {
                if (OVRInput.GetUp(grabReleaseButtons[i]))
                {
                    FreezeTexture();
                }
            }
        }
    }


    //To warp the texture we need 4 reference points; we then pass them through to a custom shader
    //to do custom transformations for pixel colours
    public Transform[] cornerTransforms; 
    public Camera centreVRAnchor; 
    
    void FreezeTexture()
    {
        Texture currentFrame = cameraAccess.GetTexture();
        Texture2D staticCopy = new Texture2D(currentFrame.width, currentFrame.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(currentFrame, staticCopy);

        //intrinsics let us work out the perspective shift between eye cam and internal camera rendering
        var intrinsics = cameraAccess.Intrinsics;

        Vector4[] corners = new Vector4[4];
        Matrix4x4 worldToCam = centreVRAnchor.worldToCameraMatrix;

        for (int i = 0; i < 4; i++)
        {
            Vector3 camSpacePoint = worldToCam.MultiplyPoint(cornerTransforms[i].position);

            //Gemini AI supported intrinsics calculations
            float xPixel = (intrinsics.FocalLength.x * (camSpacePoint.x / -camSpacePoint.z)) + intrinsics.PrincipalPoint.x;
            float yPixel = (intrinsics.FocalLength.y * (camSpacePoint.y / -camSpacePoint.z)) + intrinsics.PrincipalPoint.y;

            float u = xPixel / currentFrame.width;
            float v = yPixel / currentFrame.height;

            //Gemini intrinsics note: Depending on the OS version, you might need to flip V: v = 1.0f - v;
            corners[i] = new Vector4(u, v, 0, 0);
        }

        warpMaterial.SetTexture("_MainTex", staticCopy);
        warpMaterial.SetVectorArray("_Corners", corners);
    }
}