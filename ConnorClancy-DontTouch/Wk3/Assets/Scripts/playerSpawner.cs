using UnityEngine;
using Fusion;

public class playerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.LocalPlayer == player)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(0,1,-5),Quaternion.identity);
        }
    }
}
