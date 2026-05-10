using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Light mainLight;

    public void LightsOff()
    {
        mainLight.enabled = false;
    }

    public void LightsOn()
    {
        mainLight.enabled = true;
    }
}