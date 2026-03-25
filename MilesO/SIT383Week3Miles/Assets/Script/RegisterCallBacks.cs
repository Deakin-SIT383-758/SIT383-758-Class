using UnityEngine;
using Fusion;

public class RegisterCallbacks : MonoBehaviour
{
    void Awake()
    {
        var runner = GetComponent<NetworkRunner>();

        var spawner = GetComponent<PlayerSpawner>();
        var inputHandler = GetComponent<FusionInputHandler>();

        if (runner == null)
        {
            Debug.LogError("NetworkRunner missing!");
            return;
        }

        if (spawner != null)
            runner.AddCallbacks(spawner);
        else
            Debug.LogError("PlayerSpawner missing!");

        if (inputHandler != null)
            runner.AddCallbacks(inputHandler);
        else
            Debug.LogError("FusionInputHandler missing!");
    }
}