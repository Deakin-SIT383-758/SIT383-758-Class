using UnityEngine;

public class PinchOrbController : MonoBehaviour
{
    public Transform orb;
    public float followSpeed = 15f;
    public float normalSize = 0.08f;
    public float pinchSize = 0.16f;

    public void UpdateOrb(Vector3 indexTipPosition, bool isPinching)
    {
        if (orb == null) return;

        Vector3 targetPosition = indexTipPosition;
        targetPosition.z = -1f;

        orb.position = Vector3.Lerp(
            orb.position,
            targetPosition,
            Time.deltaTime * followSpeed
        );

        float targetSize = isPinching ? pinchSize : normalSize;
        orb.localScale = Vector3.Lerp(
            orb.localScale,
            Vector3.one * targetSize,
            Time.deltaTime * followSpeed
        );
    }
}