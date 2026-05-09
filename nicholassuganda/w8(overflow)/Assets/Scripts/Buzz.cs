using UnityEngine;
using Oculus.Haptics;

public class Buzz : MonoBehaviour
{
    public HapticClip hapticClip;
    private HapticClipPlayer player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = new HapticClipPlayer(hapticClip);
        //player.Play(Controller.Left);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float timeremaining = 0.0f;
    private void OnTriggerStay(Collider other)
    {
        timeremaining -= Time.deltaTime;
        if (timeremaining < 0.0f)
        {
            player.Play(Controller.Left);
            timeremaining = player.clipDuration;
        }
    }
}
