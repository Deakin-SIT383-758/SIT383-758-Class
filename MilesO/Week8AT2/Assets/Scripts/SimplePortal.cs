using UnityEngine;

public class SimplePortal : MonoBehaviour
{
    public GameObject virtualWorld;
    public GameObject physicalWorld;

    public Camera mainCamera;

    private bool inVirtual = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            inVirtual = !inVirtual;

            if (inVirtual)
            {
                // Enter virtual world
                mainCamera.cullingMask = LayerMask.GetMask("Virtual", "Portal");
            }
            else
            {
                // Return to physical world
                mainCamera.cullingMask = LayerMask.GetMask("Physical", "Portal");
            }
        }
    }
}