using UnityEngine;

public class Passthrough : MonoBehaviour
{
    [SerializeField] private OVRPassthroughLayer passthroughLayer;

    private bool isPassthroughOn = true;

    void Start()
    {
        if (passthroughLayer != null)
        {
            passthroughLayer.enabled = true;
            isPassthroughOn = true;
        }
    }

    void Update()
    {
        //Right controller A button only
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            TogglePassthrough();
        }
    }

    private void TogglePassthrough()
    {
        if (passthroughLayer == null)
        {
            Debug.LogWarning("Passthrough Layer is not assigned.");
            return;
        }

        isPassthroughOn = !isPassthroughOn;
        passthroughLayer.enabled = isPassthroughOn;

        Debug.Log("Passthrough is now " + (isPassthroughOn ? "ON" : "OFF"));
    }
}
