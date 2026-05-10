using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject portal;

    public void OpenPortal()
    {
        portal.SetActive(true);
    }

    public void ClosePortal()
    {
        portal.SetActive(false);
    }
}