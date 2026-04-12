using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
   public GameObject PlayerPrefab;

   public void PlayerJoined(PlayerRef player)
   {
      if (player == Runner.LocalPlayer)
      {
         Runner.Spawn(PlayerPrefab, new Vector3(1,1,1), Quaternion.identity, player);
      }
   }
}
