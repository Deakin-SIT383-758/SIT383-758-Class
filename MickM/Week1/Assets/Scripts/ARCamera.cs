using UnityEngine;
using TMPro;
using UnityEngine.Android;

public class ARCamera : MonoBehaviour
{
    public Material mat;
    private WebCamTexture camTexture;

    public WebCamTexture CamTexture { get => camTexture; set => camTexture = value; }

    private string statusString;
    public string StatusString { get => statusString; }

    private void Awake()
    {
        statusString = "Starting up";
        CamTexture = new WebCamTexture();
        mat.mainTexture = CamTexture;
        CamTexture.Play();
        statusString = "Playing";

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
            statusString = "GPS not enabled by user";
        }
        else
        {
            Input.location.Start(5f, 5f); 
        }
    }

    private Vector3 currentGps = Vector3.zero;
    public Vector3 CurrentGps { get => currentGps;}
    public Vector2 GPSAccuracy { get => new Vector2(Input.location.lastData.horizontalAccuracy, Input.location.lastData.verticalAccuracy); }


    private void Update()
    {
        float azimuth = Input.compass.trueHeading;
        Vector3 boresight = transform.forward;
        float pitch = Mathf.Asin(boresight.y) * Mathf.Rad2Deg;

        
        if (Input.location.status == LocationServiceStatus.Running)
        {
            currentGps = new Vector3(
                Input.location.lastData.latitude,
                Input.location.lastData.longitude,
                Input.location.lastData.altitude
            );

            float horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
            float verticalAccuracy = Input.location.lastData.verticalAccuracy;

            statusString = $"Loc: {CurrentGps}\n(Horiz acc:{horizontalAccuracy}, Vert acc:{verticalAccuracy})\nAz:{azimuth}, pitch:{pitch}";
        } else
        {
            if (!Input.location.isEnabledByUser)
                statusString = "GPS not enabled by user\nAz:{azimuth}, pitch:{pitch}";
            else
                statusString = $"Location service status:\n{Input.location.status}\nAz:{azimuth}, pitch:{pitch}";
        }
    }

    public Color[] GetCurrentFramePixels()
    {
        return CamTexture.GetPixels();
    }
}
