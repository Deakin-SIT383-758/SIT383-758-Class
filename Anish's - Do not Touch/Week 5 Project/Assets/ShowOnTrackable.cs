using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation.VisualScripting;

public class ShowOnTrackable : MonoBehaviour
{
    public List<GameObject> ObjectToPlace;
    private ARTrackedImageManager arTrackedManager;
    private Dictionary<string, GameObject> trackedObjects;

    private void Awake()
    {
        arTrackedManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        arTrackedManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        arTrackedManager.trackedImagesChanged -= OnImageChanged;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trackedObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < arTrackedManager.referenceLibrary.count; i++)
        {
            GameObject go = Instantiate(ObjectToPlace[i]);
            go.SetActive(false);
            trackedObjects[arTrackedManager.referenceLibrary[i].name] = go;
        }
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var addedImage in args.added)
        {
            trackedObjects[addedImage.referenceImage.name].SetActive(true);
        }

        foreach(var updatedImages in args.updated)
        {
            trackedObjects[updatedImages.referenceImage.name].SetActive(true);
            trackedObjects[updatedImages.referenceImage.name].transform.position = updatedImages.transform.position;
            trackedObjects[updatedImages.referenceImage.name].transform.rotation = updatedImages.transform.rotation;
        }

        foreach (var removedImage in args.removed)
        {
            trackedObjects[removedImage.referenceImage.name].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}