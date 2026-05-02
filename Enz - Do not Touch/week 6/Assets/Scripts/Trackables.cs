using UnityEngine;
using Meta.XR.MRUtilityKit;
using static OVRAnchor;

public class trackables : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;

    private GameObject currentCube;

    public void OnTrackableAdded(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != TrackableType.QRCode)
            return;

        if (currentCube != null)
        {
            Destroy(currentCube);
        }

        currentCube = Instantiate(spawnPrefab, trackable.transform);
        currentCube.transform.localPosition = Vector3.zero;
        currentCube.transform.localRotation = Quaternion.identity;
    }

    public void OnTrackableRemoved(MRUKTrackable trackable)
    {
        ClearCube();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            ClearCube();
        }
    }

    private void ClearCube()
    {
        if (currentCube != null)
        {
            Destroy(currentCube);
            currentCube = null;
        }
    }
}