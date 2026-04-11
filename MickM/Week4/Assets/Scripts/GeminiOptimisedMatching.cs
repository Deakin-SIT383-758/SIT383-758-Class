using UnityEngine;

[RequireComponent(typeof(KeypointMatcher))]
public class GeminiOptimisedMatching : MonoBehaviour
{
    private KeypointMatcher matchingScript;
    void Start()
    {
        matchingScript = GetComponent<KeypointMatcher>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            matchingScript.SetSimilarityMatchingFunction(GetRotationInvariantPoseSimilarity);
        }
    }

    //This method contains the Gemini AI optimised version of rotation invariant similarity 
    //that is found in KeypointMatcher.cs
    public float GetRotationInvariantPoseSimilarity(Vector3[] current, Vector3[] saved)
    {
        // 1. Pre-calculate inverses once per hand
        Quaternion invCur = Quaternion.Inverse(matchingScript.GetHandRotation(current));
        Quaternion invSav = Quaternion.Inverse(matchingScript.GetHandRotation(saved));

        float totalError = 0;
        Vector3 wristCur = current[0];
        Vector3 wristSav = saved[0];

        for (int i = 1; i < current.Length; i++)
        {
            // 2. Localize and Normalize in one go
            // Rotating the direction vector is faster than rotating the point and then subtracting
            Vector3 localCur = invCur * (current[i] - wristCur).normalized;
            Vector3 localSav = invSav * (saved[i] - wristSav).normalized;

            totalError += (1f - Vector3.Dot(localCur, localSav));
        }

        return totalError;
    }
}
