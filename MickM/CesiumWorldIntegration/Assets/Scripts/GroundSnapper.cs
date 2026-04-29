using Unity;
using UnityEngine;

public class GroundSnapper : MonoBehaviour
{
    public Transform xrOriginPivot;
    public Transform vrCamera;
    public LayerMask terrainLayer;

    public float landingSmoothness = 5f;
    public float basePlayerHeight = 0f;

    private void Update()
    {
        UpdateGroundSnapping();
    }

    private void UpdateGroundSnapping()
    {
        Vector3 rayStart = new Vector3(vrCamera.position.x, 50000f, vrCamera.position.z);

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100000f, terrainLayer))
        {
            float terrainHeightY = hit.point.y;
            float targetPivotY = terrainHeightY + basePlayerHeight;
            float currentScale = xrOriginPivot.localScale.y;

            //If we are getting close to normal scale we make sure we are gounded
            if (currentScale <= 20.0f) 
            {
                Vector3 currentPos = xrOriginPivot.position;
                float newY = Mathf.Lerp(currentPos.y, targetPivotY, Time.deltaTime * landingSmoothness);
                xrOriginPivot.position = new Vector3(currentPos.x, newY, currentPos.z);
            }
        }
    }
}