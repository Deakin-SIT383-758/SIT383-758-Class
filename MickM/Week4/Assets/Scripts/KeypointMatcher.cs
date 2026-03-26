using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(HandDetection))]
public class KeypointMatcher : MonoBehaviour
{
    public bool checkPinch = true;
    private HandDetection handDetection;
    [SerializeField] private float pinchThreshold = 0.15f;
    [SerializeField] private List<Vector3[]> savedJointPositions;

    //This delegate will allow simple swap in/out of similarity approaches.
    //Input: The two vector arrays we are comparing (ie. recorded pose, current pose)
    //Output: Matching score - 0 is a perfect match.
    private System.Func<Vector3[], Vector3[], float> SimilarityMatchingFunction;

    private void Awake()
    {
        handDetection = GetComponent<HandDetection>();
        savedJointPositions = new List<Vector3[]>();

        SimilarityMatchingFunction = SumSquareDistances;
    }

    private bool recordPositionsFlag = false;
    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            recordPositionsFlag = true;
        }
    }

    bool isPinching = false;
    int closestIndexMatch = -1;
    float closestIndexScore = 0;
    private void HandDetection_FrameDetectionCompleteEvent(Vector3[] jointPositions)
    {
        if (recordPositionsFlag)
            RecordPositions(jointPositions);

        if (checkPinch)
            CheckPinch(jointPositions);
        else
            isPinching = false;

        closestIndexMatch = GetNearestIndex(jointPositions);
    }

    public float detectionThreshold = 0.1f;
    int GetNearestIndex(Vector3[] jointPositions)
    {
        if (savedJointPositions.Count == 0) return -1;

        float bestSimilarity = 999;
        int bestIndex = -1;
        for (int i = 0; i < savedJointPositions.Count; i++)
        {
            float indexScore = SimilarityMatchingFunction(jointPositions, savedJointPositions[i]);
            if (indexScore < detectionThreshold && indexScore < bestSimilarity)
            {
                bestSimilarity = indexScore;
                bestIndex = i;
            }
        }
        closestIndexScore = bestSimilarity;
        return bestIndex;
    }

    float SumSquareDistances(Vector3[] a,  Vector3[] b)
    {
        float totalDistance = 0;

        for (int i = 0; i < a.Length; i++)
        {
            totalDistance += (a[i] - b[i]).sqrMagnitude;
        }

        return totalDistance;
    }


    void OnGUI()
    {
        string s = isPinching ? "Pinching" : "Not pinching";
        s += "\n";
        if (closestIndexMatch < 0)
        {
            s += "No matching pose found";
        } else
        {
            s += $"Matching pose index: {closestIndexMatch} ({closestIndexScore})";
        }
        GUI.Label(new Rect(10, 10, 300, 200), s);

    }
    private void RecordPositions(Vector3[] jointPositions)
    {
        recordPositionsFlag = false;
        savedJointPositions.Add(jointPositions);

        Debug.Log($"Saved joint positions: {savedJointPositions.Count}. Positions:\n{savedJointPositions}");
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
