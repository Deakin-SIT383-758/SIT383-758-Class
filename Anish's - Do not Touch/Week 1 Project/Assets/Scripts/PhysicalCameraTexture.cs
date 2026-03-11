using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class PhysicalCameraTexture : MonoBehaviour
{
    public Material camTexMaterial;

    private WebCamTexture webcamTexture;

    void Update()
    {
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

        if (webcamTexture != null && !webcamTexture.isPlaying)
        {
            camTexMaterial.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
    }
}