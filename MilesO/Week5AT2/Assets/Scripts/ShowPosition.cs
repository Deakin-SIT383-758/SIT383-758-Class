using UnityEngine;

public class ShowPosition : MonoBehaviour
{
    public GPSTracking gps;
    public GameObject markerPrefab;
    public Transform earthtransform;

    public float globeRadius = 0.5f;

    private GameObject marker;
    private float radius;
    private bool markerPlaced;

    void Start()
    {
        MeshRenderer renderer = earthtransform.GetComponent<MeshRenderer>();
        radius = renderer.bounds.extents.x;
    }

    void Update()
    {
        float latitude, longitude, altitude;


        if (!markerPlaced && gps.retrieveLocation(out latitude, out longitude, out altitude))
        {
            Vector3 position = globeRadius *
                new Vector3(
                    Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Cos(longitude * Mathf.Deg2Rad),
                    Mathf.Sin(latitude * Mathf.Deg2Rad),
                    Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Sin(longitude * Mathf.Deg2Rad)
                );

            if (marker == null)
            {
                marker = Instantiate(markerPrefab);
                marker.transform.SetParent(transform, false);
                marker.transform.localScale = Vector3.one * 0.05f;
            }

            marker.transform.localPosition = position;
            marker.transform.up = transform.TransformDirection(position);

            markerPlaced = true;
        }
    }
}