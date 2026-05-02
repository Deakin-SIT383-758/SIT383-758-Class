using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    public GameObject viewSphere;  
    public Material MonoMat;
    public Material StereoMat;
    bool StereoActive = true;
    bool MonoActive;
    Renderer ThreeSixty;

    void Start()
    {
        ThreeSixty = viewSphere.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A) && MonoActive == true)
        {
            ThreeSixty.material = StereoMat;
            StereoActive = true;
            MonoActive = false;
        }
        else if (OVRInput.GetDown(OVRInput.RawButton.A) && StereoActive == true)
        {
            ThreeSixty.material = MonoMat;
            StereoActive = false;
            MonoActive = true;
        }
    }
}
