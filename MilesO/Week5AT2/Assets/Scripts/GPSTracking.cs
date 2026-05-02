using UnityEngine;
using UnityEngine.Android;

public class GPSTracking : MonoBehaviour
{
    public bool fakeLocation = false; // for testing in editor/ on PC

    public bool retrieveLocation(out float latitude, out float longitude, out float altitude)
    {
        latitude = 0f;
        longitude = 0f;
        altitude = 0f;

        // Fake mode (for testing in editor)
        if (fakeLocation)
        {
            latitude = -37.8136f;   // Melbourne!!!
            longitude = 144.9631f;
            altitude = 0f;
            return true;
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Requesting permission...");
            Permission.RequestUserPermission(Permission.FineLocation);
            return false;
        }

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location NOT enabled on device");
            return false;
        }

        if (Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("Starting GPS...");
            Input.location.Start();
            return false;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.Log("GPS still initializing...");
            return false;
        }

        // 🔹 Get data
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        altitude = Input.location.lastData.altitude;

        return true;
    }


}
