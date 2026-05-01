using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ShowOnTrackable : MonoBehaviour
{
    public List<GameObject> ObjectToPlace;
    public Color[] tapColors = { Color.white, Color.cyan, Color.red, Color.green, Color.yellow };
    public float tapScaleMultiplier = 1.4f;
    public float animDuration = 0.25f;

    public float bobHeight = 0.015f;
    public float bobSpeed = 1.5f;

    public float positionLerpSpeed = 12f;

    private ARTrackedImageManager arTrackedManager;
    private Camera arCamera;

    private Dictionary<string, GameObject> trackedObjects = new();
    private Dictionary<string, Vector3> basePositions = new();
    private Dictionary<string, Quaternion> baseRotations = new();
    private Dictionary<string, int> colorIndices = new();
    private Dictionary<string, Vector3> originalScales = new();

    void Awake()
    {
        arTrackedManager = GetComponent<ARTrackedImageManager>();
        arCamera = Camera.main;
    }

    void OnEnable()
    {
        arTrackedManager.trackedImagesChanged += OnImageChanged;
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        arTrackedManager.trackedImagesChanged -= OnImageChanged;
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        for (int i = 0; i < arTrackedManager.referenceLibrary.count; i++)
        {
            string name = arTrackedManager.referenceLibrary[i].name;
            GameObject go = Instantiate(ObjectToPlace[i]);
            go.SetActive(false);
            trackedObjects[name] = go;
            originalScales[name] = go.transform.localScale;
            colorIndices[name] = 0;
        }
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var img in args.added)
        {
            string name = img.referenceImage.name;
            var go = trackedObjects[name];
            basePositions[name] = img.transform.position;
            baseRotations[name] = img.transform.rotation;
            go.transform.SetPositionAndRotation(img.transform.position, img.transform.rotation);
            go.SetActive(true);
            StartCoroutine(PopIn(name, go));
        }

        foreach (var img in args.updated)
        {
            string name = img.referenceImage.name;
            trackedObjects[name].SetActive(true);
            basePositions[name] = img.transform.position;
            baseRotations[name] = img.transform.rotation;
        }

        foreach (var img in args.removed)
            trackedObjects[img.referenceImage.name].SetActive(false);
    }

    void Update()
    {
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        foreach (var kvp in trackedObjects)
        {
            if (!kvp.Value.activeInHierarchy) continue;
            string name = kvp.Key;

            if (basePositions.TryGetValue(name, out Vector3 basePos))
            {
                Vector3 targetPos = basePos + Vector3.up * bob;
                kvp.Value.transform.position = Vector3.Lerp(kvp.Value.transform.position, targetPos, Time.deltaTime * positionLerpSpeed);
            }

            if (baseRotations.TryGetValue(name, out Quaternion baseRot))
                kvp.Value.transform.rotation = baseRot;
        }

        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                HandleTap(touch.screenPosition);
                break;
            }
        }
    }

    void HandleTap(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        foreach (var kvp in trackedObjects)
        {
            if (hit.transform == kvp.Value.transform || hit.transform.IsChildOf(kvp.Value.transform))
            {
                CycleColor(kvp.Key, kvp.Value);
                StartCoroutine(BounceScale(kvp.Key, kvp.Value));
                break;
            }
        }
    }

    void CycleColor(string name, GameObject go)
    {
        colorIndices[name] = (colorIndices[name] + 1) % tapColors.Length;
        foreach (var r in go.GetComponentsInChildren<Renderer>())
            r.material.color = tapColors[colorIndices[name]];
    }

    IEnumerator PopIn(string name, GameObject go)
    {
        Vector3 target = originalScales[name];
        go.transform.localScale = Vector3.zero;
        for (float t = 0; t < animDuration; t += Time.deltaTime)
        {
            go.transform.localScale = Vector3.Lerp(Vector3.zero, target, t / animDuration);
            yield return null;
        }
        go.transform.localScale = target;
    }

    IEnumerator BounceScale(string name, GameObject go)
    {
        Vector3 original = originalScales[name];
        Vector3 big = original * tapScaleMultiplier;
        float half = animDuration / 2f;

        for (float t = 0; t < half; t += Time.deltaTime)
        {
            go.transform.localScale = Vector3.Lerp(original, big, t / half);
            yield return null;
        }
        for (float t = 0; t < half; t += Time.deltaTime)
        {
            go.transform.localScale = Vector3.Lerp(big, original, t / half);
            yield return null;
        }
        go.transform.localScale = original;
    }
}
