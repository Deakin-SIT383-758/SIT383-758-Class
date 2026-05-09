using UnityEngine;
using TMPro;
using System.Collections;

public class VoiceCameraController : MonoBehaviour, ISpeechToTextListener
{
    public Transform mainCamera;

    public Transform frontMarker;
    public Transform backMarker;
    public Transform leftMarker;
    public Transform rightMarker;

    public TMP_Text statusText;

    public float rotationSpeed = 2f;

    private Coroutine currentRotation;

    void Awake()
    {
        // Initialize speech recognition
        SpeechToText.Initialize("en-US");
    }

    void Start()
    {
        statusText.text =
            "Press button and say Front, Back, Left or Right";
    }

    public void StartListening()
    {
        SpeechToText.RequestPermissionAsync((permission) =>
        {
            if (permission == SpeechToText.Permission.Granted)
            {
                bool started = SpeechToText.Start(this);

                if (started)
                {
                    statusText.text = "Listening...";
                }
                else
                {
                    statusText.text =
                        "Couldn't start speech recognition";
                }
            }
            else
            {
                statusText.text =
                    "Microphone permission denied";
            }
        });
    }

    void RotateTo(Transform target)
    {
        if (target == null)
            return;

        if (currentRotation != null)
            StopCoroutine(currentRotation);

        currentRotation =
            StartCoroutine(SmoothRotate(target));
    }

    IEnumerator SmoothRotate(Transform target)
    {
        Quaternion startRot = mainCamera.rotation;

        Quaternion endRot =
            Quaternion.LookRotation(
                target.position - mainCamera.position
            );

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;

            mainCamera.rotation =
                Quaternion.Slerp(
                    startRot,
                    endRot,
                    t
                );

            yield return null;
        }
    }

    // =========================
    // Speech Recognition Events
    // =========================

    public void OnReadyForSpeech()
    {
        Debug.Log("Ready for speech");
    }

    public void OnBeginningOfSpeech()
    {
        Debug.Log("Speech started");
    }

    public void OnVoiceLevelChanged(float normalizedVoiceLevel)
    {
        // Optional visual feedback
    }

    public void OnPartialResultReceived(string spokenText)
    {
        statusText.text = spokenText;
    }

    public void OnResultReceived(string spokenText, int? errorCode)
    {
        Debug.Log(
            "Speech Result: " + spokenText +
            (errorCode.HasValue
                ? (" Error: " + errorCode)
                : "")
        );

        if (string.IsNullOrEmpty(spokenText))
        {
            statusText.text = "No speech detected";
            return;
        }

        spokenText = spokenText.ToLower();

        statusText.text = "Heard: " + spokenText;

        if (spokenText.Contains("front"))
        {
            RotateTo(frontMarker);
        }
        else if (spokenText.Contains("back"))
        {
            RotateTo(backMarker);
        }
        else if (spokenText.Contains("left"))
        {
            RotateTo(leftMarker);
        }
        else if (spokenText.Contains("right"))
        {
            RotateTo(rightMarker);
        }
    }
}