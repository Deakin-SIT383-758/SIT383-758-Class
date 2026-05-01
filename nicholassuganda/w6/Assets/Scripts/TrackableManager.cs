using UnityEngine;
using TMPro;
using Meta.XR.MRUtilityKit;
using static OVRAnchor;

public class TrackableManager : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private float labelHeight = 0.12f;
    [SerializeField] private float labelFontSize = 0.06f;

    public void OnTrackableAdded(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != TrackableType.QRCode)
            return;

        GameObject go = Instantiate(spawnPrefab, trackable.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        string content = trackable.MarkerPayloadString;
        if (!string.IsNullOrEmpty(content))
            AttachLabel(go, content);
    }

    void AttachLabel(GameObject parent, string text)
    {
        GameObject labelObj = new GameObject("QRLabel");
        labelObj.transform.SetParent(parent.transform, false);
        labelObj.transform.localPosition = new Vector3(0f, labelHeight, 0f);

        TextMeshPro tmp = labelObj.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = labelFontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        RectTransform rt = labelObj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0.4f, 0.15f);
    }
}
