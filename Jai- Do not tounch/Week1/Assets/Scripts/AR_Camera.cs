using UnityEngine;
using TMPro;

public class AR_Camera : MonoBehaviour
{
    public Material cubeMaterail;
    private WebCamTexture WCTexture;
    private int currentCamIndex = 0;
    private WebCamDevice[] devices;
    public TextMeshProUGUI ButtonText;
    public TextMeshProUGUI SpecialText;
    private string allDevices;
    void Start()
    {
        devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            ButtonText.SetText(devices[currentCamIndex].name);
            PlayCamera(currentCamIndex);

            for (int i = 0; i < devices.Length;)
            {
                allDevices += devices[i].name + ", ";
                i++;
            }
            SpecialText.SetText(allDevices);
        }
    }

    public void SwitchCamera()
    {
        if (devices.Length == 0) return;

        currentCamIndex = (currentCamIndex + 1) % devices.Length;
        ButtonText.SetText(devices[currentCamIndex].name);
        PlayCamera(currentCamIndex);
    }

    void PlayCamera(int index)
    {
        if (WCTexture != null)
        {
            WCTexture.Stop();
        }

        WebCamDevice device = devices[index];
        WCTexture = new WebCamTexture(device.name);
        ButtonText.SetText(device.name);
        cubeMaterail.mainTexture = WCTexture;
        WCTexture.Play();
    }
}