using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerStarter : MonoBehaviour
{
    public NetworkRunner runner;

    private async void Start()
    {
        if (runner == null)
            runner = GetComponent<NetworkRunner>();

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            Scene = scene,
            SessionName = "MySession"
        });
    }
}
