using UnityEngine;

public class ARCamera : MonoBehaviour
{
    public Material cubeMaterial;

    private WebCamTexture wcTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wcTexture = new WebCamTexture();

        cubeMaterial.mainTexture = wcTexture;
        wcTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
