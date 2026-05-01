using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Android;
using System.Collections;

public class ARCamera : MonoBehaviour
{
    WebCamTexture wcTexture;
    public Material cubeMat;
    public TextMeshProUGUI words;

    private void PermissionCallbacksPermissionGranted(string permissionName)
    {
        StartCoroutine(DelayedCameraInitialization());
    }

    private IEnumerator DelayedCameraInitialization()
    {
        yield return null;
        IntialiseCamera();
    }

    private void PermissionCallbacksPermissionDenied(string permissionName)
    {
        Debug.LogWarning($"Permission {permissionName} Denied");
    }

    private void AskCameraPermission()
    {
        var callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += PermissionCallbacksPermissionDenied;
        callbacks.PermissionGranted += PermissionCallbacksPermissionGranted;
        Permission.RequestUserPermission(Permission.Camera, callbacks);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (UnityEngine.InputSystem.Gyroscope.current != null) 
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            AskCameraPermission();
            return;
        }

        IntialiseCamera();
        
        words.text = "Hello World";
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.InputSystem.Gyroscope gyro = UnityEngine.InputSystem.Gyroscope.current;

        // Check if the device is available and enabled
        if (gyro != null && gyro.enabled)
        {
            Vector3 angularVelocity = gyro.angularVelocity.ReadValue();
            words.text = "Gyroscope angular velocity: " + angularVelocity;
        }
    }

    void IntialiseCamera()
    {
        wcTexture = new WebCamTexture();
        cubeMat.mainTexture = wcTexture;
        wcTexture.Play();
    }
}
