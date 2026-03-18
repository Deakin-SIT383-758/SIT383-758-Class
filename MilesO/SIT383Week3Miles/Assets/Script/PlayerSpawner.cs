using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined //Function from Photon for multiplayer
{
    public GameObject PlayerPrefab; //Prefab for the player character

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.LocalPlayer == player) //Check if the player that joined is the local player
        {
            Runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity); //Spawn the player character at the origin with no rotation
        }
    }
}
