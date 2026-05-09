using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeechManager : MonoBehaviour, ISpeechToTextListener
{
    public TMP_Text transcriptText;
    public CommandManager commandManager;

    public Button listenButton;

    private void Start()
    {
        SpeechToText.Initialize("en-US");
    }

    public void StartListening()
    {
        Debug.Log("Requesting microphone permission...");
        SpeechToText.RequestPermissionAsync((permission) =>
        {
            Debug.Log("Permission: " + permission);

            if (permission == SpeechToText.Permission.Granted)
            {
                bool started = SpeechToText.Start(this);
                Debug.Log("Speech started: " + started);
                transcriptText.text = "Listening...";
                listenButton.interactable = false;
            }
            else
            {
                transcriptText.text = "Microphone permission denied";
            }
        });
    }

    void ISpeechToTextListener.OnReadyForSpeech()
    {
        Debug.Log("Ready for speech");
    }

    void ISpeechToTextListener.OnBeginningOfSpeech()
    {
        Debug.Log("Speech started");
    }

    void ISpeechToTextListener.OnVoiceLevelChanged(float level)
    {
    }

    void ISpeechToTextListener.OnPartialResultReceived(string spokenText)
    {
        transcriptText.text = spokenText;
    }

    void ISpeechToTextListener.OnResultReceived(string spokenText, int? errorCode)
    {
        transcriptText.text = "You said: " + spokenText;

        commandManager.ProcessCommand(spokenText);

        Invoke(nameof(RestartListening), 1f);

        listenButton.interactable = true;
    }

    void RestartListening()
    {
        if (!SpeechToText.IsBusy())
        {
            StartListening();
        }
    }
}