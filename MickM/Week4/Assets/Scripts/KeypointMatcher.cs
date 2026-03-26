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

        SetSimilarityMatchingFunction(SumSquareDistances);
    }

    private bool recordPositionsFlag = false;
    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            recordPositionsFlag = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSimilarityMatchingFunction(SumSquareDistances);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSimilarityMatchingFunction(GetRotationInvariantPoseSimilarity);
        }
    }

    public void SetSimilarityMatchingFunction(System.Func<Vector3[], Vector3[], float> matchingFunction)
    {
        SimilarityMatchingFunction = matchingFunction;
        Debug.Log(matchingFunction.Method.Name);
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

    #region Pose Detection Methods
    float SumSquareDistances(Vector3[] a,  Vector3[] b)
    {
        float totalDistance = 0;

        for (int i = 0; i < a.Length; i++)
        {
            totalDistance += (a[i] - b[i]).sqrMagnitude;
        }

        return totalDistance;
    }


    //To get rotation invariant pose similarity we need to:
    //1. Determine the rotation of each set of points
    //2. Get the vector from the wrist to each point in each hand pose
    //3. Translate that into local space through inverse quartenion
    //4. Take the dot product; match is 1 so add (1-dot product) to error
    public float GetRotationInvariantPoseSimilarity(Vector3[] current, Vector3[] saved)
    {
        Quaternion currentRot = GetHandRotation(current);
        Quaternion savedRot = GetHandRotation(saved);

        float totalError = 0;

        for (int i = 1; i < current.Length; i++)
        {
            Vector3 currentVec = (current[i] - current[0]).normalized;
            Vector3 savedVec = (saved[i] - saved[0]).normalized;

            Vector3 localCur = Quaternion.Inverse(currentRot) * currentVec;
            Vector3 localSav = Quaternion.Inverse(savedRot) * savedVec;

            totalError += (1 - Vector3.Dot(localCur, localSav));
        }

        return totalError;
    }

    private Quaternion GetHandRotation(Vector3[] joints)
    {
        //Define rotations based on Forward, Right and Up
        //Forward: Wrist (0) to base of middle finger (mcp) (9)
        //Right: Base of index (5) to base of pinky (17)
        //Up: Cross product of Forward and Right
        Vector3 forward = (joints[9] - joints[0]).normalized;
        Vector3 right = (joints[17] - joints[5]).normalized;
        Vector3 up = Vector3.Cross(forward, right);

        return Quaternion.LookRotation(forward, up);
    }
    #endregion

    void OnGUI()
    {
        string s = isPinching ? "Pinching" : "Not pinching";
        s += $"\nMatching function: {GetMatchingFunctionName()}\n";
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

    public string GetMatchingFunctionName()
    {
        return $"{SimilarityMatchingFunction.Method.DeclaringType.Name}.{SimilarityMatchingFunction.Method.Name}";
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
