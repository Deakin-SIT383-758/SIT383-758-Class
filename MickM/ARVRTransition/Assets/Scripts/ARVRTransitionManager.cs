using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARVRTransitionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OVRCameraRig _cameraRig;
    [SerializeField] private OVRPassthroughLayer _passthroughLayer;

    [Header("Transition Settings")]
    [SerializeField] private float _transitionSpeed = 1.5f; 

    [Header("Perspective Settings")]
    [SerializeField] private Vector3 _vrPositionOffset = new Vector3(0, 5f, -2f);
    [SerializeField] private float _vrWorldScale = 5.0f;

    [Header("Visibility Lists")]
    [SerializeField] private List<GameObject> _arOnlyObjects;
    [SerializeField] private List<GameObject> _vrOnlyObjects;

    private float _transitionProgress = 0; // 0 = AR, 1 = VR
    private Vector3 _initialRigPos;
    private CharacterController _charController; 

    void Awake()
    {
        _initialRigPos = _cameraRig.transform.localPosition;

        _charController = _cameraRig.GetComponent<CharacterController>();
        if (_charController == null) _charController = _cameraRig.GetComponentInChildren<CharacterController>();
    }

    public OVRInput.Button transitionButton;

    void Update()
    {
        bool holdingTransitionButton = OVRInput.Get(transitionButton);

        if (holdingTransitionButton)
        {
            _transitionProgress = Mathf.MoveTowards(_transitionProgress, 1.0f, Time.deltaTime * _transitionSpeed);
        }
        else
        {
            _transitionProgress = Mathf.MoveTowards(_transitionProgress, 0.0f, Time.deltaTime * _transitionSpeed);
        }

        ApplyTransition(_transitionProgress);
    }

    private void ApplyTransition(float t)
    {
        float smoothedT = Mathf.SmoothStep(0, 1, t);
        if (_charController != null)
        {
            _charController.enabled = (t < 0.01f);
        }

        //Opacity
        _passthroughLayer.textureOpacity = 1.0f - smoothedT;

        //Position
        _cameraRig.transform.localPosition = Vector3.Lerp(_initialRigPos, _initialRigPos + _vrPositionOffset, smoothedT);

        //Scaling for "toy" look
        float currentScale = Mathf.Lerp(1.0f, _vrWorldScale, smoothedT);
        _cameraRig.transform.localScale = Vector3.one * currentScale;


        //We only want AR when we are pretty much exactly in normal scale, ie. t = 0
        bool showVR = t > 0.1f;
        foreach (var obj in _arOnlyObjects) if (obj.activeSelf == showVR) obj.SetActive(!showVR);
        foreach (var obj in _vrOnlyObjects) if (obj.activeSelf != showVR) obj.SetActive(showVR);
    }
}