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
    [SerializeField] private Slider redSlider;
    [SerializeField] private Slider greenSlider;
    [SerializeField] private Slider blueSlider;


    void Start()
    {
        transform.root.gameObject.SetActive(false); // hide UI to start
    }

    public void PlayerJoined()
    {
        transform.root.gameObject.SetActive(true); // display UI on joining
    }

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

    public Color GetSlidersColour()
    {
        Debug.Log("Red slider value: " + redSlider.value);
        Debug.Log("Green slider value: " + greenSlider.value);
        Debug.Log("Blue slider value: " + blueSlider.value);
        Color newColour = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        return newColour;
    }

    public void Submit()
    {
        GetPlayer();
        custom.ChangeName(nameField.text);
        custom.ChangeColor(GetSlidersColour());
        transform.root.gameObject.SetActive(false); // hide UI after submission
    }
}
