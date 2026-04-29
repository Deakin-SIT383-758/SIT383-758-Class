using UnityEngine;
using Oculus.Haptics;

public class HapticBuzz : MonoBehaviour
{
    public HapticClip hapClip;
    HapticClipPlayer player;
    float timeRemaining = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = new HapticClipPlayer(hapClip);
        //player.Play(Controller.Left);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OTriggerStay(Collider other)
    {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining < 0.0f)
        {
            player.Play(Controller.Left);
            timeRemaining = player.clipDuration;
        }
    }
}
