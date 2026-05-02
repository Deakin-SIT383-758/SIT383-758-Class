using UnityEngine;
using TMPro;

public class GPSDisplay : MonoBehaviour
{
    public GPSTracking gps;
    public TextMeshProUGUI displayText;

    void Update()
    {
        Debug.Log("ShowPosition running");
        if (gps == null)
        {
            Debug.Log("GPS reference is NULL");
            return;
        }
        float lat, lon, alt;

        if (gps.retrieveLocation(out lat, out lon, out alt))
        {
            displayText.text =
                "Latitude: " + lat.ToString("F6") + "\n" +
                "Longitude: " + lon.ToString("F6") + "\n" +
                "Altitude: " + alt.ToString("F2");
        }
        else
        {
            displayText.text = "Getting GPS...";
        }
    }
}