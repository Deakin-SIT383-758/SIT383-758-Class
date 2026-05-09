using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class VoiceCube : MonoBehaviourPun
{
    private Recorder recorder;
    private Renderer rend;

    public float speakingThreshold = 0.08f;

    void Start()
    {
        recorder = GetComponent<Recorder>();
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (recorder == null || recorder.LevelMeter == null)
            return;

        float volume = recorder.LevelMeter.CurrentPeakAmp;

        rend.material.color = volume > speakingThreshold ? Color.green : Color.red;
    }
}