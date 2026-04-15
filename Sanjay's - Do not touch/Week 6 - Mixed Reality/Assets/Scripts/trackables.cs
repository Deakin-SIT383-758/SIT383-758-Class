using UnityEngine;
using Meta.XR.MRUtilityKit;
using static OVRAnchor;

public class trackables : MonoBehaviour
{
    [SerializeField] private GameObject spwanPrefab;

    public void OnTrackableAdded(MRUKTrackable trackable)
    {
        if(trackable.TrackableType != TrackableType.QRCode)
        {
            return;
        }

        GameObject go = Instantiate(spwanPrefab, trackable.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
    }
}
