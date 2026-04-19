using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionFixer : MonoBehaviour
{
    [SerializeField] ARSession arSession;
    public TextMeshProUGUI debugText;

    IEnumerator Start()
    {
        debugText.text = "START CALLED";
        if (ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.Installing)
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            Debug.LogError("AR is not supported on this device.");
            debugText.text = "AR is not supported on this device";
        }
        else
        {
            // Force the session to enable
            arSession.enabled = true;
            debugText.text = "arSession.enabled = true";
        }

        StartCoroutine(ToggleARSession());
    }

    bool toggling = false;
    IEnumerator ToggleARSession()
    {
        toggling = true;
        yield return new WaitForSeconds(2);
        debugText.text = "Toggling AR Session - OFF";
        arSession.enabled = false;
        yield return new WaitForSeconds(2);

        debugText.text = "Toggling AR Session - ON";
        arSession.enabled = true;
        yield return new WaitForSeconds(1);
        toggling = false;
    }

    void Update()
    {
        if (toggling) return;

        debugText.text = $"Session State: {ARSession.state}\n" +
                         $"Tracking State: {ARSession.notTrackingReason}";
    }
}