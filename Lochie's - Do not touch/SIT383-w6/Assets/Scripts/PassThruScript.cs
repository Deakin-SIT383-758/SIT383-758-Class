using Unity.VisualScripting;
using UnityEngine;

public class PassThruScript : MonoBehaviour
{
    [SerializeField] private OVRPassthroughLayer passThruLayer;

    GameObject[] partialThruObjects;
    GameObject[] fullThruObjects;

    bool fullPassThruEnabled = true;
    bool partialPassThruEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (passThruLayer != null)
        {
            fullPassThruEnabled = true;
            partialPassThruEnabled = true;
            passThruLayer.enabled = true;
        }

        if (partialThruObjects == null && fullThruObjects == null)
        {
            partialThruObjects = GameObject.FindGameObjectsWithTag("HalfThru");
            fullThruObjects = GameObject.FindGameObjectsWithTag("FullThru");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            ToggleFullPassThru();
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            TogglePartialPassThru();
        }
    }

    void ToggleFullPassThru()
    {
        if(passThruLayer == null)
        {
            Debug.Log("PassThru not Assigned");
            return;
        }

        fullPassThruEnabled = !fullPassThruEnabled;
        passThruLayer.enabled = fullPassThruEnabled;

        foreach (GameObject gameObject in partialThruObjects)
        {
            gameObject.SetActive(fullPassThruEnabled);
        }
        foreach (GameObject gameObject in fullThruObjects)
        {
            gameObject.SetActive(fullPassThruEnabled);
        }
    }

    void TogglePartialPassThru()
    {
        if(passThruLayer == null)
        {
            Debug.Log("PassThru not Assigned");
            return;
        }

        partialPassThruEnabled = !partialPassThruEnabled;
        passThruLayer.enabled = partialPassThruEnabled;

        foreach (GameObject gameObject in partialThruObjects)
        {
            gameObject.SetActive(partialPassThruEnabled);
        }
    }
}
