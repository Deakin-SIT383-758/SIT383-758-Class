using System.Collections;
using UnityEngine;

public class Passthrough : MonoBehaviour
{
    [SerializeField] private OVRPassthroughLayer passthroughlayer;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float thumbstickSensitivity = 1.5f;

    private bool isPassthroughOn = true;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (passthroughlayer != null)
        {
            passthroughlayer.enabled = true;
            passthroughlayer.textureOpacity = 1f;
            isPassthroughOn = true;
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            TogglePassthrough();
        }

        HandleThumbstick();
    }

    void HandleThumbstick()
    {
        if (passthroughlayer == null) return;

        float axis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
        if (Mathf.Abs(axis) < 0.1f) return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        float newOpacity = Mathf.Clamp01(passthroughlayer.textureOpacity + axis * thumbstickSensitivity * Time.deltaTime);
        passthroughlayer.textureOpacity = newOpacity;
        passthroughlayer.enabled = newOpacity > 0f;
        isPassthroughOn = newOpacity > 0.5f;
    }

    void TogglePassthrough()
    {
        if (passthroughlayer == null)
        {
            Debug.Log("Passthrough layer is not assigned");
            return;
        }

        isPassthroughOn = !isPassthroughOn;

        OVRInput.SetControllerVibration(0.3f, 0.3f, OVRInput.Controller.RTouch);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadePassthrough(isPassthroughOn ? 1f : 0f));
    }

    IEnumerator FadePassthrough(float targetOpacity)
    {
        passthroughlayer.enabled = true;
        float startOpacity = passthroughlayer.textureOpacity;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            passthroughlayer.textureOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsed / fadeDuration);
            yield return null;
        }

        passthroughlayer.textureOpacity = targetOpacity;
        passthroughlayer.enabled = targetOpacity > 0f;
    }
}
