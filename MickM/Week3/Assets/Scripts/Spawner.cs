using UnityEngine;
using Fusion;
public class Spawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefab;

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        if(Runner.IsServer == false)
        {
            return;
        }

        Runner.Spawn(playerPrefab, new Vector3(
                Random.Range(-5, 5),
                0,
                Random.Range(-5, 5)
            ),
            Quaternion.identity);
    }
}
