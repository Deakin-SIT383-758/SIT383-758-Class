using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPre;

    public void PlayerJoined(PlayerRef player)
    {
        if(Runner.LocalPlayer == player)
        {
            Runner.Spawn(PlayerPre, new Vector3(0,1,2), Quaternion.identity);
        }
    }
}
