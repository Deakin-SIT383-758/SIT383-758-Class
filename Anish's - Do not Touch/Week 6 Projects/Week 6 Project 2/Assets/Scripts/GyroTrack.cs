using UnityEngine;
using TMPro;

public class GyroTrack : MonoBehaviour
{
    public TMP_Text debugText;
    private Gyroscope gyro;
    private bool gyroSupported;
    private float neutralTilt;
    private bool calibrated = false;
    private float calibrationDelay = 1.0f;

    void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;

        if (gyroSupported)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        UpdateDebugText("Hold phone straight...");
    }

    void Update()
    {
        if (!gyroSupported)
        {
            UpdateDebugText("Gyro not supported");
            return;
        }

        float tilt = gyro.attitude.eulerAngles.z;

        transform.rotation = Quaternion.Euler(0f, 0f, 90f - tilt);

        if (!calibrated && Time.time > calibrationDelay)
        {
            neutralTilt = tilt;
            calibrated = true;
        }

        if (!calibrated)
        {
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
            UpdateDebugText("Calibrating...\nHold phone straight");
            return;
        }

        float correctedTilt =
            Mathf.DeltaAngle(neutralTilt, tilt);

        float xGravity =
            -Mathf.Sin(correctedTilt * Mathf.Deg2Rad) * 9.81f;

        Physics.gravity = new Vector3(
            xGravity,
            -9.81f,
            0f
        );

        UpdateDebugText("");
    }

    private void UpdateDebugText(string message)
    {
        Debug.Log(message);

        if (debugText != null)
        {
            debugText.text = message;
        }
    }
}