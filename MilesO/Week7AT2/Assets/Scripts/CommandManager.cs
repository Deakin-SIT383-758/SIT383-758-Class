using UnityEngine;
using TMPro;

public class CommandManager : MonoBehaviour
{
    public Light roomLight;

    public GameObject cubePrefab;
    public Transform spawnPoint;

    public TMP_Text statusText;

    private GameObject currentCube;

    public void ProcessCommand(string command)
    {
        command = command.ToLower();

        // LIGHT COMMANDS
        if (command.Contains("lights on"))
        {
            roomLight.enabled = true;
            statusText.text = "Lights turned on";
        }
        else if (command.Contains("lights off"))
        {
            roomLight.enabled = false;
            statusText.text = "Lights turned off";
        }

        // SPAWN CUBE
        else if (command.Contains("spawn cube"))
        {
            SpawnCube();
        }

        // COLOUR COMMANDS
        else if (command.Contains("red"))
        {
            ChangeCubeColor(Color.red);
        }
        else if (command.Contains("blue"))
        {
            ChangeCubeColor(Color.blue);
        }
        else if (command.Contains("green"))
        {
            ChangeCubeColor(Color.green);
        }
    }

    void SpawnCube()
    {
        if (currentCube != null)
        {
            Destroy(currentCube);
        }

        currentCube = Instantiate(cubePrefab,
                                   spawnPoint.position,
                                   Quaternion.identity);

        statusText.text = "Cube spawned";
    }

    void ChangeCubeColor(Color color)
    {
        if (currentCube != null)
        {
            Renderer renderer = currentCube.GetComponent<Renderer>();

            renderer.material.color = color;

            statusText.text = "Cube colour changed";
        }
        else
        {
            statusText.text = "No cube exists";
        }
    }
}