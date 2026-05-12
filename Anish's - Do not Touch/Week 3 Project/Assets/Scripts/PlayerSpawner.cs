using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (!Runner.IsServer)
            return;

        Vector3 spawnPosition = new Vector3(0, 1, -2);

        NetworkObject playerObject = Runner.Spawn(
            PlayerPrefab,
            spawnPosition,
            Quaternion.identity,
            player
        );

        Runner.SetPlayerObject(player, playerObject);
    }
}