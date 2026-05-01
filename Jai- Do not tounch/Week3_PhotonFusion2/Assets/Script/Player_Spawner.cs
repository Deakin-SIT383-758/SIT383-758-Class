using UnityEngine;
using Fusion;

public class Player_Spawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(-1.0f, 2.75f, 0.0f), Quaternion.identity, player);
        }
    }

}
