using UnityEngine;
using Oculus.Haptics;
using Unity.VisualScripting;

public class HapticBuzz : MonoBehaviour
{
    public GameObject leftObject;
    public GameObject rightObject;
    public HapticClip rightHapClip;
    public HapticClip leftHapClip;
    HapticClipPlayer RightPlayer;
    HapticClipPlayer LeftPlayer;
    float timeRemaining = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RightPlayer = new HapticClipPlayer(rightHapClip);
        LeftPlayer = new HapticClipPlayer(leftHapClip);
        //player.Play(Controller.Left);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining < 0.0f)
        {
            if(other.gameObject == leftObject)
            {
                LeftPlayer.Play(Controller.Left);
                timeRemaining = LeftPlayer.clipDuration;
            }
            else if(other.gameObject == rightObject)
            {
                RightPlayer.Play(Controller.Right);
                timeRemaining = RightPlayer.clipDuration;
            }
        }
    }
}
