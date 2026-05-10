using UnityEngine;
using Oculus.Haptics;
public class Buzz : MonoBehaviour
{
    public HapticClip hapClipfar;
    public HapticClip hapClipmedium;
    public HapticClip hapClipclose;
    public HapticClip hapClipon;
    private HapticClipPlayer close;
    private HapticClipPlayer medium;
    private HapticClipPlayer far;
    private HapticClipPlayer on;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        close = new HapticClipPlayer(hapClipclose);
        medium = new HapticClipPlayer(hapClipmedium);
        far = new HapticClipPlayer(hapClipfar);
        on = new HapticClipPlayer(hapClipon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float timeremaining = 0f;
    private void OnTriggerStay(Collider other)
    {
        timeremaining -= Time.deltaTime;
        if (other.GetComponent<TargetZone>() != null)
        {

            if (timeremaining < 0f)
            {
                if (other.GetComponent<TargetZone>().Target == 0)
                {
                    on.Play(Controller.Left);
                    timeremaining = on.clipDuration;
                    Destroy(other.transform.parent.gameObject);
                }
                else if (other.GetComponent<TargetZone>().Target == 1)
                {
                    close.Play(Controller.Left);
                    timeremaining = on.clipDuration;
                }
                else if (other.GetComponent<TargetZone>().Target == 2)
                {
                    medium.Play(Controller.Left);
                    timeremaining = on.clipDuration;
                }
                else if (other.GetComponent<TargetZone>().Target == 3)
                {
                    far.Play(Controller.Left);
                    timeremaining = on.clipDuration;
                }
            }
        }
    }
}
