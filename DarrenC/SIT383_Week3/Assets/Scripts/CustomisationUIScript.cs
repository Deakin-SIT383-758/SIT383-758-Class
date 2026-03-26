using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fusion;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomisationUIScript : NetworkBehaviour
{
    public List<NetworkObject> players;
    [SerializeField] private NetworkObject player;
    private PlayerCustomisation custom;

    [SerializeField] private TMP_InputField nameField;

    public void GetPlayer()
    {
        foreach (PlayerMovement pm in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None)) // iterate through all player prefabs
        {
            players.Add(pm.gameObject.GetComponent<NetworkObject>());
            if (pm.HasStateAuthority) // find prefab in which we have state authority (i.e. our player prefab)
            {
                player = pm.gameObject.GetComponent<NetworkObject>();
                custom = player.GetComponent<PlayerCustomisation>();
            }
        }
    }
    public void Submit()
    {
        GetPlayer();
        custom.ChangeName(nameField.text);
    }
}
