using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public SpawnManager spawnManager;
    public EnvironmentManager environmentManager;
    public PortalManager portalManager;

    public void ProcessCommand(string command)
    {
        Debug.Log("Processing: " + command);

        if (command.Contains("spawn cube"))
        {
            spawnManager.SpawnCube();
        }



        else if (command.Contains("lights off"))
        {
            environmentManager.LightsOff();
        }

        else if (command.Contains("lights on"))
        {
            environmentManager.LightsOn();
        }

        else if (command.Contains("open portal"))
        {
            portalManager.OpenPortal();
        }
    }
}