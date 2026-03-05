using UnityEngine;

public class ARCamera : MonoBehaviour
{
    public Material cubeMaterial;

    private WebCamTexture wcTexture;
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
