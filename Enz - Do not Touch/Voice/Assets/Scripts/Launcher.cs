using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon");

        PhotonNetwork.JoinOrCreateRoom(
            "DemoRoom",
            new RoomOptions { MaxPlayers = 4 },
            TypedLobby.Default
        );
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        Vector3 spawnPosition = new Vector3(
            Random.Range(-2f, 2f),
            1f,
            Random.Range(-2f, 2f)
        );

        PhotonNetwork.Instantiate(
            playerPrefab.name,
            spawnPosition,
            Quaternion.identity
        );
    }
}