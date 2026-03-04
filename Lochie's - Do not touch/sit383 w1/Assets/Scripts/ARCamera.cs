using UnityEngine;
using TMPro;

public class ARCamera : MonoBehaviour
{
    WebCamTexture wcTexture;
    public Material cubeMat;
    public TextMeshProUGUI words;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wcTexture = new WebCamTexture();
        cubeMat.mainTexture = wcTexture;
        wcTexture.Play();

        words.text = "Hello World";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
