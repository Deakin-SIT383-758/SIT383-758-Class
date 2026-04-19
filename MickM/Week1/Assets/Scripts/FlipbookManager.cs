using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlipbookManager : MonoBehaviour
{
    private ARCamera arCamera;
    private List<FlipbookFrameData> savedFrames = new List<FlipbookFrameData>();

    public TextMeshProUGUI debugText;

    private void Awake()
    {
        arCamera = GetComponent<ARCamera>();
    }

    private void Start()
    {
        flipbookUpdateCountdown = flipbookUpdateHz;
        flipbookTexture = new Texture2D(arCamera.CamTexture.width, arCamera.CamTexture.height);

    }
    public Material imageDisplayMat;
    private Texture2D flipbookTexture;
    public void SaveImage()
    {
        var pixels = arCamera.GetCurrentFramePixels();
        var gpsPos = arCamera.CurrentGps;
        Vector2 acc = arCamera.GPSAccuracy;
        FlipbookFrameData frameData = new FlipbookFrameData()
        {
            pixels = pixels,
            gpsPosition = gpsPos,
            horizontalAccuracy = acc.x,
            verticalAccuracy = acc.y
        };

        savedFrames.Add(frameData);
    }

    public void TogglePlayFlipbook()
    {
        playFlipbook = !playFlipbook;
    }

    public readonly float flipbookUpdateHz = 0.5f;
    private float flipbookUpdateCountdown;
    private bool playFlipbook = false;
    private void Update()
    {
        if (playFlipbook == false)
        {
            debugText.text = arCamera.StatusString;
            return;
        }
        flipbookUpdateCountdown -= Time.deltaTime;
        if (flipbookUpdateCountdown > 0) return;

        flipbookUpdateCountdown = flipbookUpdateHz;
        FlipImage();
    }

    private int currentIndex = 0;
    private void FlipImage()
    {
        if (savedFrames == null || savedFrames.Count == 0) return;
        currentIndex = (currentIndex + 1) % savedFrames.Count;

        flipbookTexture.SetPixels(savedFrames[currentIndex].pixels);
        flipbookTexture.Apply();
        imageDisplayMat.mainTexture = flipbookTexture;

        debugText.text = savedFrames[currentIndex].StatusString();
    }

    public struct FlipbookFrameData
    {
        public Color[] pixels;
        public Vector3 gpsPosition;

        public float horizontalAccuracy;
        public float verticalAccuracy;

        public float azimuth;
        public float pitch;

        public string StatusString()
        {
            return $"IMAGE METADATA:\nLoc: {gpsPosition}\n(Horiz acc:{horizontalAccuracy}, Vert acc:{verticalAccuracy})\nAz:{azimuth}, pitch:{pitch}";
        }
    }
}
