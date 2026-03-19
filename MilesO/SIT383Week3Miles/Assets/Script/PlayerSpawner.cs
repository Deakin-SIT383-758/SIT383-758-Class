using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        // Only the host (State Authority) is allowed to spawn players
        if (Runner.IsServer)
        {
            Runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity, player);
        }

        Debug.Log("PlayerJoined fired for: " + player); //debug log to confirm the function is being called
    }
}
