using UnityEngine;

public class Passthrough : MonoBehaviour
{
[SerializeField] private OVRPassthroughLayer passthroughlayer;

    private bool isPassthroughOn = true;
    void Start()
    {
        if (passthroughlayer != null)
        {

            passthroughlayer.enabled = true;
            isPassthroughOn = true;
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            TogglePassthrough();
        }
    }

    void TogglePassthrough()
    {
        if (passthroughlayer == null)
        {
            Debug.Log("Passthrough layer is not assigned");
            return;
        }

        isPassthroughOn = !isPassthroughOn;
        passthroughlayer.enabled = isPassthroughOn;
    }
}
