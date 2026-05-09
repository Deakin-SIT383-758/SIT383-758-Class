using UnityEngine;
using Oculus.Haptics;

public class Buzz : MonoBehaviour
{
    public HapticClip hapticClip;
    public Controller controller = Controller.Left;

    private HapticClipPlayer player;
    private float timeRemaining = 0.0f;

    void Start()
    {
        player = new HapticClipPlayer(hapticClip);
    }

    private void OnTriggerStay(Collider other)
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining < 0.0f)
        {
            player.Play(controller);
            timeRemaining = player.clipDuration;
        }
    }
}