using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;


    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.LocalPlayer == player)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(5, 1, -2), Quaternion.identity);
        }
    }
}