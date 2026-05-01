using UnityEngine;

public class Turnscript : MonoBehaviour
{
    public float turnSpeed = 100f;
    public float verticalSpeed = 60f;
    public float maxPitch = 80f;

    public bool useSnapTurn = false;
    public float snapAngle = 45f;
    [SerializeField] private float snapCooldown = 0.35f;

    [SerializeField] private float zoomedAmount = 0.35f;
    [SerializeField] private float zoomLerpSpeed = 6f;

    [SerializeField] private float adjustSpeed    = 1.2f;
    [SerializeField] private float defaultExposure   = 1f;
    [SerializeField] private float defaultSaturation = 1f;
    [SerializeField] private float highContrastValue = 1.6f;

    private InputSystem_Actions control;
    private Material photoMaterial;
    private float snapTimer;
    private float currentYaw;
    private float currentPitch;
    private float currentZoom       = 1f;
    private float currentExposure;
    private float currentSaturation;
    private float currentContrast   = 1f;
    private bool  highContrast      = false;

    void Start()
    {
        control = new InputSystem_Actions();
        control.Enable();
        currentYaw   = transform.eulerAngles.y;
        currentPitch = 0f;

        currentExposure   = defaultExposure;
        currentSaturation = defaultSaturation;

        photoMaterial = GameObject.Find("PhotoSphere")?.GetComponent<Renderer>()?.material;
        if (photoMaterial != null)
        {
            photoMaterial.SetFloat("_Zoom",       1f);
            photoMaterial.SetFloat("_Exposure",   currentExposure);
            photoMaterial.SetFloat("_Saturation", currentSaturation);
            photoMaterial.SetFloat("_Contrast",   currentContrast);
        }
    }

    void Update()
    {
        Vector2 move = control.Player.Move.ReadValue<Vector2>();
        HandleHorizontalTurn(move.x);
        HandleVerticalLook(move.y);
        HandleZoom();
        HandleSnapToggle();
        HandlePhotoAdjust();
    }

    void HandleHorizontalTurn(float x)
    {
        if (useSnapTurn)
        {
            snapTimer -= Time.deltaTime;
            if (Mathf.Abs(x) > 0.6f && snapTimer <= 0f)
            {
                currentYaw += Mathf.Sign(x) * snapAngle;
                snapTimer = snapCooldown;
                HapticsManager.PulseBoth(0.3f, 0.06f);
            }
        }
        else
        {
            currentYaw += x * turnSpeed * Time.deltaTime;
        }
        ApplyRotation();
    }

    void HandleVerticalLook(float y)
    {
        currentPitch -= y * verticalSpeed * Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch, -maxPitch, maxPitch);
        ApplyRotation();
    }

    // Stores yaw/pitch separately to avoid euler angle ambiguity
    void ApplyRotation() => transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

    void HandleZoom()
    {
        if (photoMaterial == null) return;
        bool gripping = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) ||
                        OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        float target = gripping ? zoomedAmount : 1f;
        currentZoom = Mathf.Lerp(currentZoom, target, Time.deltaTime * zoomLerpSpeed);
        photoMaterial.SetFloat("_Zoom", currentZoom);
    }

    void HandleSnapToggle()
    {
        // A button (right controller)
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            useSnapTurn = !useSnapTurn;
            HapticsManager.PulseBoth(0.4f, 0.08f);
        }
    }

    void HandlePhotoAdjust()
    {
        if (photoMaterial == null) return;

        // Right thumbstick: Y = exposure, X = saturation
        Vector2 stick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (stick.sqrMagnitude > 0.05f)
        {
            currentExposure   = Mathf.Clamp(currentExposure   + stick.y * adjustSpeed * Time.deltaTime, 0f, 3f);
            currentSaturation = Mathf.Clamp(currentSaturation + stick.x * adjustSpeed * Time.deltaTime, 0f, 2f);
            photoMaterial.SetFloat("_Exposure",   currentExposure);
            photoMaterial.SetFloat("_Saturation", currentSaturation);
        }

        // Y button (left controller) — toggle high contrast
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            highContrast   = !highContrast;
            currentContrast = highContrast ? highContrastValue : 1f;
            photoMaterial.SetFloat("_Contrast", currentContrast);
            HapticsManager.PulseBoth(0.3f, 0.08f);
        }

        // B button (right controller) — reset all adjustments
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            currentExposure   = defaultExposure;
            currentSaturation = defaultSaturation;
            currentContrast   = 1f;
            highContrast      = false;
            photoMaterial.SetFloat("_Exposure",   currentExposure);
            photoMaterial.SetFloat("_Saturation", currentSaturation);
            photoMaterial.SetFloat("_Contrast",   currentContrast);
            HapticsManager.PulseBoth(0.6f, 0.18f);
        }
    }

    void OnDestroy() => control?.Disable();
}
