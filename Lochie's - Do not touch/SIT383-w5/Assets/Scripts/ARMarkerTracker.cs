using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using System;
using TMPro;

public class ARMarkerTracker : MonoBehaviour
{
    public List<GameObject> ObjectsToPlace;
    Dictionary<string, GameObject> trackedObjects;
    ARTrackedImageManager arTrackedManager;
    public TextMeshProUGUI debug;

    void Awake()
    {
        arTrackedManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        arTrackedManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        arTrackedManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var addedImage in args.added)
        {
            trackedObjects[addedImage.referenceImage.name].SetActive(true);
            debug.text += "Added Image";
        }

        foreach (var updatedImage in args.updated)
        {
            trackedObjects[updatedImage.referenceImage.name].SetActive(true);

            trackedObjects[updatedImage.referenceImage.name].transform.position = updatedImage.transform.position;
            trackedObjects[updatedImage.referenceImage.name].transform.rotation = updatedImage.transform.rotation;
            debug.text += "Updated Image";
        }

        foreach (var removedImage in args.removed)
        {
            trackedObjects[removedImage.referenceImage.name].SetActive(false);
            debug.text += "Removed Image";
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trackedObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < arTrackedManager.referenceLibrary.count; i++)
        {
            GameObject go = Instantiate(ObjectsToPlace[i]);
            go.SetActive(false);
            trackedObjects[arTrackedManager.referenceLibrary[i].name] = go;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
