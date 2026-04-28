using CesiumForUnity;
using Oculus.Platform;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CesiumController : MonoBehaviour
{
    private int[] mapIDs = new[] { 2, 4 };
    private int currentMapID = 0;

    public CesiumGeoreference geoReference;
    public float maxScale = 200000;
    public float basePanSpeed = 0.5f;

    [SerializeField] private CesiumIonRasterOverlay rasterOverlay;
    private void Awake()
    {
        if (rasterOverlay == null)
            rasterOverlay = GameObject.FindFirstObjectByType<CesiumIonRasterOverlay>();

        if (rasterOverlay == null)
        {
            Debug.LogError("CESIUM ION RASTER OVERLAY IS NULL", gameObject);
            this.enabled = false;
            return;
        }

        rasterOverlay.ionAssetID = mapIDs[currentMapID];
    }

    public Transform playerTransform;
    public Camera playerCamera;
    private float playerScale = 1f;
    public float playerFullZoomTime = 5f;
    private float zoomTuningSpeed = 1 / 100f;
    private float playerScalingModifier
    {
        get
        {
            return zoomTuningSpeed * maxScale / playerFullZoomTime; 
        }
    }

    private void Update()
    {
        Vector2 scrollValue = GetVRMoveInput();
        ScrollMap(scrollValue);

        float zoom = GetVRZoomInput();
        float zoomDelta = playerScale * zoom * Time.deltaTime * playerScalingModifier * zoomTuningSpeed;

        playerScale = Mathf.Clamp(playerScale + zoomDelta, 1, maxScale);
        UpdatePlayerScale(playerScale);

        CheckMapToggle();
    }




    private void CheckMapToggle()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SwapMapRendering();
            }
        }

        if (OVRInput.GetDown(mapToggleButton))
        {
            SwapMapRendering();
        }
    }

    public void UpdatePlayerScale(float playerScale)
    {
        playerTransform.localScale = playerScale * Vector3.one;
        playerCamera.nearClipPlane = 0.3f * playerScale;
        playerCamera.farClipPlane = 1000 * playerScale;
    }


    //Stop going too far south or north...
    public float maxNorthLatitude = 85.0f; 
    public float maxSouthLatitude = -85.0f;
    public void ScrollMap(Vector2 scrollInput)
    {
        if (scrollInput.sqrMagnitude > 0.01f)
        {
            float cameraYaw = playerCamera.transform.eulerAngles.y;
            Quaternion yawRotation = Quaternion.Euler(0, cameraYaw, 0);
            Vector3 input3D = new Vector3(scrollInput.x, 0, scrollInput.y);

            Vector3 moveDir = yawRotation * input3D;
            float scaleFactor = playerScale/10000f;

            double dynamicSpeed = basePanSpeed * scaleFactor * Time.deltaTime;

            
            // Gemini support to work out a nice constant speed near poles
            double cosLat = System.Math.Max(0.05, System.Math.Cos(geoReference.latitude * Mathf.Deg2Rad));
            double newLongitude = geoReference.longitude + ((moveDir.x * dynamicSpeed) / cosLat);
            double newLatitude = geoReference.latitude + (moveDir.z * dynamicSpeed);

            if (newLongitude > 180.0) newLongitude -= 360.0;
            if (newLongitude < -180.0) newLongitude += 360.0;
            newLatitude = System.Math.Clamp(newLatitude, maxSouthLatitude, maxNorthLatitude);

            geoReference.longitude = newLongitude;
            geoReference.latitude = newLatitude;
        }
    }

    [ContextMenu("Toggle map overlays")]
    public void SwapMapRendering()
    {
        currentMapID = (currentMapID + 1) % mapIDs.Length;
        rasterOverlay.ionAssetID = mapIDs[currentMapID];
    }

    #region OVRControl
    public OVRInput.Button zoomOutButton, zoomInButton;
    public OVRInput.Button mapToggleButton;
    private Vector2 GetVRMoveInput()
    {
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
    }

    private float GetVRZoomInput()
    {
        if (OVRInput.Get(zoomOutButton))
            return 1.0f;
        if (OVRInput.Get(zoomInButton))
            return -1.0f;
        return 0f;
    }
    #endregion
}
