using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            var playerObject = Runner.Spawn(playerPrefab, GetRandomSpawnPoint(), Quaternion.identity, player);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-10, 10),
            0,
            Random.Range(-10, 10)
            );
    }
}
