using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Transform mainCamera;

    public Transform frontMarker;
    public Transform backMarker;
    public Transform leftMarker;
    public Transform rightMarker;

    public float rotationSpeed = 2f;

    private Coroutine currentRotation;

    public void LookFront() => RotateTo(frontMarker);
    public void LookBack() => RotateTo(backMarker);
    public void LookLeft() => RotateTo(leftMarker);
    public void LookRight() => RotateTo(rightMarker);

    void RotateTo(Transform target)
    {
        if (target == null) return;

        if (currentRotation != null)
            StopCoroutine(currentRotation);

        currentRotation = StartCoroutine(SmoothRotate(target));
    }

    System.Collections.IEnumerator SmoothRotate(Transform target)
    {
        Quaternion startRot = mainCamera.rotation;
        Quaternion endRot = Quaternion.LookRotation(target.position - mainCamera.position);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            mainCamera.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
    }
}
