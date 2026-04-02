using UnityEngine;
using TMPro;
using UnityEngine.Android;

public class ARCamera : MonoBehaviour
{
    public Material mat;
    private WebCamTexture camTexture;
    public TextMeshProUGUI debugText;

    public WebCamTexture CamTexture { get => camTexture; set => camTexture = value; }

    private void Awake()
    {
        debugText.text = "Starting up";
        CamTexture = new WebCamTexture();
        mat.mainTexture = CamTexture;
        CamTexture.Play();
        debugText.text = "Playing";

        Input.compass.enabled = true;
        Input.gyro.enabled = true;

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

    }

    private void Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            debugText.text = "GPS not enabled by user";
        }
        else
        {
            Input.location.Start(5f, 5f); 
        }
    }

    private void Update()
    {
        float azimuth = Input.compass.trueHeading;
        Vector3 boresight = transform.forward;
        float pitch = Mathf.Asin(boresight.y) * Mathf.Rad2Deg;

        Vector3 currentGps = Vector3.zero;
        if (Input.location.status == LocationServiceStatus.Running)
        {
            currentGps = new Vector3(
                Input.location.lastData.latitude,
                Input.location.lastData.longitude,
                Input.location.lastData.altitude
            );

            float horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
            float verticalAccuracy = Input.location.lastData.verticalAccuracy;

            debugText.text = $"Loc: {currentGps} (Horiz acc:{horizontalAccuracy}, Vert acc:{verticalAccuracy})\nAz:{azimuth}, pitch:{pitch}";
        } else
        {
            if (!Input.location.isEnabledByUser)
                debugText.text = "GPS not enabled by user\nAz:{azimuth}, pitch:{pitch}";
            else
                debugText.text = $"Location service status:\n{Input.location.status}\nAz:{azimuth}, pitch:{pitch}";
        }
    }

    public Color[] GetCurrentFramePixels()
    {
        return CamTexture.GetPixels();
    }
}
