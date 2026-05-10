using UnityEngine;

public class Passthrough : MonoBehaviour
{
    [SerializeField] private OVRPassthroughLayer passthroughlayer;

    private bool isPassthroughOn = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (passthroughlayer != null)
        {
            passthroughlayer.enabled = true;
            isPassthroughOn = true;
        }

    }
    // Update is called once per frame
    void Update()
    {
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                TogglePassthru();
            }
    }

    void TogglePassthru()
    {
        if (passthroughlayer == null)
        {
            Debug.Log("Passthru no assigned");
            return;
        }

        isPassthroughOn = !isPassthroughOn;
        passthroughlayer.enabled = isPassthroughOn;
    }
}
