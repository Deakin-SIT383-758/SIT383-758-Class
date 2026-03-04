using UnityEngine;
using TMPro;

public class ARCamera : MonoBehaviour
{
    public Material mat;
    private WebCamTexture camTexture;
    public TextMeshProUGUI debugText;

    public WebCamTexture CamTexture { get => camTexture; set => camTexture = value; }

    private void Awake()
    {
        debugText.text = "Starting up";
        CamTexture = new WebCamTexture();
        mat.mainTexture = CamTexture;
        CamTexture.Play();
        debugText.text = "Playing";
    }

    public Color[] GetCurrentFramePixels()
    {
        return CamTexture.GetPixels();
    }
}
