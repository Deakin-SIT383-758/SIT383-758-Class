using UnityEngine;

public class AR_Camera : MonoBehaviour
{
    public Material cubeMaterail;
    private WebCamTexture WCTexture;
    void Start()
    {
        WCTexture = new WebCamTexture();
        cubeMaterail.mainTexture = WCTexture;
        WCTexture.Play();
    }


    void Update()
    {

    }
}
