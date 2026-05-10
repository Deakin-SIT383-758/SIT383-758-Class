using UnityEngine;
using Meta.XR.MRUtilityKit;
using static OVRAnchor;
public class Trackables : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;

    public void OnTrackableAdded(MRUKTrackable trackable)
    {
        if(trackable.TrackableType != TrackableType.QRCode)
        {
            return;
        }

        GameObject go = Instantiate(spawnPrefab, trackable.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        
    }
}
