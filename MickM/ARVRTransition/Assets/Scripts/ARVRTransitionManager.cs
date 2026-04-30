using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARVRTransitionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OVRCameraRig cameraRig;
    [SerializeField] private OVRPassthroughLayer passthroughLayer;
    [SerializeField] private Camera centreVRCamera;


    [Header("Transition Settings")]
    [SerializeField] private float transitionSpeed = 1.5f;

    [Header("Perspective Settings")]
    [SerializeField] private Transform vrPositionTarget; 
    [SerializeField] private float vrWorldScale = 5.0f;
    private Vector3 vrPositionOffset;
    private float nearClipInitial, farClipInitial;

    [Header("Visibility Lists")]
    [SerializeField] private List<GameObject> _arOnlyObjects;
    [SerializeField] private List<GameObject> _vrOnlyObjects;

    private float transitionProgress = 0; // 0 = AR, 1 = VR
    private Vector3 initialRigPos;

    void Awake()
    {
        nearClipInitial = centreVRCamera.nearClipPlane;
        farClipInitial = centreVRCamera.farClipPlane;
        SetLerpPositions();
    }

    void SetLerpPositions()
    {
        initialRigPos = cameraRig.transform.localPosition;
        vrPositionOffset = vrPositionTarget.position - initialRigPos;
    }

    public OVRInput.Button transitionButton;

    void Update()
    {
        if (OVRInput.GetDown(transitionButton))
        {
            SetLerpPositions();
        }

        bool holdingTransitionButton = OVRInput.Get(transitionButton);
        if (holdingTransitionButton)
        {
            transitionProgress = Mathf.MoveTowards(transitionProgress, 1.0f, Time.deltaTime * transitionSpeed);
        }
        else
        {
            transitionProgress = Mathf.MoveTowards(transitionProgress, 0.0f, Time.deltaTime * transitionSpeed);
        }

        ApplyTransition(transitionProgress);
    }

    private void ApplyTransition(float t)
    {
        float smoothedT = Mathf.SmoothStep(0, 1, t);

        //Opacity
        passthroughLayer.textureOpacity = 1.0f - smoothedT;

        //Position
        cameraRig.transform.localPosition = Vector3.Lerp(initialRigPos, initialRigPos + vrPositionOffset, smoothedT);

        //Scaling for "toy" look
        float currentScale = Mathf.Lerp(1.0f, vrWorldScale, smoothedT);
        cameraRig.transform.localScale = Vector3.one * currentScale;


        //We only want AR when we are pretty much exactly in normal scale, ie. t = 0
        bool showVR = t > 0.1f;
        foreach (var obj in _arOnlyObjects) if (obj.activeSelf == showVR) obj.SetActive(!showVR);
        foreach (var obj in _vrOnlyObjects) if (obj.activeSelf != showVR) obj.SetActive(showVR);

        //Update camera clip planes
        centreVRCamera.nearClipPlane = Mathf.Lerp(nearClipInitial, 0.01f, smoothedT);
        centreVRCamera.farClipPlane = Mathf.Lerp(farClipInitial, 20f, smoothedT);
    }
}