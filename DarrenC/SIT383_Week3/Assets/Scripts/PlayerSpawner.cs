using UnityEngine;
using Fusion;
using TMPro;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.LocalPlayer == player)
        {
            NetworkObject newPlayer = Runner.Spawn(playerPrefab, new Vector3(0, 1, 0));
        }
    }
}
