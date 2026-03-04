using System.Collections.Generic;
using UnityEngine;

public class FlipbookManager : MonoBehaviour
{
    private ARCamera arCamera;
    private List<Color[]> savedTextures = new List<Color[]>();

    private void Awake()
    {
        arCamera = GetComponent<ARCamera>();
    }

    private void Start()
    {
        flipbookUpdateCountdown = flipbookUpdateHz;
        flipbookTexture = new Texture2D(arCamera.CamTexture.width, arCamera.CamTexture.height);

    }
    public Material testMat;
    private Texture2D flipbookTexture;
    public void SaveImage()
    {
        //flipbookTexture = new Texture2D(arCamera.CamTexture.width, arCamera.CamTexture.height);

        var pixels = arCamera.GetCurrentFramePixels();
        savedTextures.Add(pixels);
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
        if (playFlipbook == false) return;
        flipbookUpdateCountdown -= Time.deltaTime;
        if (flipbookUpdateCountdown > 0) return;

        flipbookUpdateCountdown = flipbookUpdateHz;
        FlipImage();

    }

    private int currentIndex = 0;
    private void FlipImage()
    {
        if (savedTextures == null || savedTextures.Count == 0) return;
        currentIndex = (currentIndex + 1) % savedTextures.Count;

        flipbookTexture.SetPixels(savedTextures[currentIndex]);
        flipbookTexture.Apply();
        testMat.mainTexture = flipbookTexture;
    }
}
