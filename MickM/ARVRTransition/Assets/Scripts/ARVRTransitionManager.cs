using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARVRTransitionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OVRCameraRig _cameraRig;
    [SerializeField] private OVRPassthroughLayer _passthroughLayer;

    [Header("Transition Settings")]
    [SerializeField] private float _transitionSpeed = 1.5f; // Speed of the slide

    [Header("Perspective Settings")]
    [SerializeField] private Vector3 _vrPositionOffset = new Vector3(0, 5f, -2f);
    [SerializeField] private float _vrWorldScale = 5.0f;

    [Header("Visibility Lists")]
    [SerializeField] private List<GameObject> _arOnlyObjects;
    [SerializeField] private List<GameObject> _vrOnlyObjects;

    private float _transitionProgress = 0; // 0 = AR, 1 = VR
    private Vector3 _initialRigPos;
    private CharacterController _charController; // To disable gravity/physics

    void Awake()
    {
        _initialRigPos = _cameraRig.transform.localPosition;
        // Check if you have a CharacterController (added by Player Controller block)
        _charController = _cameraRig.GetComponent<CharacterController>();
        if (_charController == null) _charController = _cameraRig.GetComponentInChildren<CharacterController>();
    }

    void Update()
    {
        // 1. Check Input (Hold to go to VR)
        bool isHolding = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) ||
                         OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);

        // 2. Update Progress Value
        if (isHolding)
        {
            _transitionProgress = Mathf.MoveTowards(_transitionProgress, 1.0f, Time.deltaTime * _transitionSpeed);
        }
        else
        {
            _transitionProgress = Mathf.MoveTowards(_transitionProgress, 0.0f, Time.deltaTime * _transitionSpeed);
        }

        // 3. Apply the Transition Logic
        ApplyTransition(_transitionProgress);
    }

    private void ApplyTransition(float t)
    {
        // Ease the 't' value for a smoother feel (S-Curve)
        float smoothedT = Mathf.SmoothStep(0, 1, t);

        // Disable physics/gravity if we aren't fully in AR
        if (_charController != null)
        {
            _charController.enabled = (t < 0.01f);
        }

        // 1. Passthrough Opacity
        _passthroughLayer.textureOpacity = 1.0f - smoothedT;

        // 2. Camera Rig Position
        _cameraRig.transform.localPosition = Vector3.Lerp(_initialRigPos, _initialRigPos + _vrPositionOffset, smoothedT);

        // 3. Camera Rig Scale (Diorama Effect)
        float currentScale = Mathf.Lerp(1.0f, _vrWorldScale, smoothedT);
        _cameraRig.transform.localScale = Vector3.one * currentScale;

        // 4. Object Visibility (Swap at the 50% mark)
        bool showVR = t > 0.5f;
        foreach (var obj in _arOnlyObjects) if (obj.activeSelf == showVR) obj.SetActive(!showVR);
        foreach (var obj in _vrOnlyObjects) if (obj.activeSelf != showVR) obj.SetActive(showVR);
    }
}