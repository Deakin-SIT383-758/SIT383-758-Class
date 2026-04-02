using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultipleMarkerObjects : MonoBehaviour
{
    public List<GameObject> objectPrefabs;
    private Dictionary<string, GameObject> trackedObjects;
    private ARTrackedImageManager arTrackedManager;

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

    void Start()
    {
        trackedObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < arTrackedManager.referenceLibrary.count; i++)
        {
            GameObject go = Instantiate(objectPrefabs[i]);
            go.SetActive(false);
            trackedObjects[arTrackedManager.referenceLibrary[i].name] = go;
        }
    }
    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var addedImage in args.added)
        {
            trackedObjects[addedImage.referenceImage.name].SetActive(true);
        }

        foreach (var updatedImage in args.updated)
        {
            trackedObjects[updatedImage.referenceImage.name].SetActive(true);
            trackedObjects[updatedImage.referenceImage.name].transform.position = updatedImage.transform.position;
            trackedObjects[updatedImage.referenceImage.name].transform.rotation = updatedImage.transform.rotation;
        }

        foreach (var removedImage in args.removed)
        {
            trackedObjects[removedImage.referenceImage.name].SetActive(false);
        }
    }
}
