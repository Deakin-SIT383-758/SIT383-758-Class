using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(HandDetection))]
public class KeypointMatcher : MonoBehaviour
{
    public bool checkPinch = true;
    private HandDetection handDetection;
    [SerializeField] private float pinchThreshold = 0.15f;

    private void Awake()
    {
        handDetection = GetComponent<HandDetection>();
    }


    bool isPinching = false;
    private void HandDetection_FrameDetectionCompleteEvent(Vector3[] jointPositions)
    {
        if (checkPinch)
            CheckPinch(jointPositions);
        else
            isPinching = false;
    }

    void OnGUI()
    {
        string s = isPinching ? "Pinching" : "Not pinching";
        GUI.Label(new Rect(10, 10, 300, 200), s);

    }

    void CheckPinch(Vector3[] jointPositions)
    {
        Vector3 wrist = jointPositions[0];
        Vector3 thumbTip = jointPositions[4];
        Vector3 indexTip = jointPositions[8];

        float normalisedFingertipDistance = (indexTip - thumbTip).magnitude / (thumbTip - wrist).magnitude;

        isPinching = (normalisedFingertipDistance < pinchThreshold);
    }



    private void OnEnable()
    {
        handDetection.FrameDetectionCompleteEvent += HandDetection_FrameDetectionCompleteEvent;
    }

    private void OnDisable()
    {
        handDetection.FrameDetectionCompleteEvent -= HandDetection_FrameDetectionCompleteEvent;
    }
}
