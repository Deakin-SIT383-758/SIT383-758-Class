using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCustomisation : NetworkBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TextMeshPro nameTag;

    // Network synchronised variables, ensures name tags and colours are updated for all players
    [Networked, OnChangedRender(nameof(ColorChanged))]
    public Color networkedColor { get; set; }

    [Networked, OnChangedRender(nameof(NameChanged))]
    public string networkedName { get; set; }

    void ColorChanged()
    {
        meshRenderer.material.SetColor("_Color", networkedColor);
    }

    void NameChanged()
    {
        nameTag.text = networkedName;
    }

    public void ChangeColor(Color newColor)
    {
        if (Object.HasStateAuthority) networkedColor = newColor; // only allow with state authority (ensures only colour of this player is changed)
    }


    public void ChangeName(string newName)
    {
        if (Object.HasStateAuthority) networkedName = newName; // only allow with state authority (ensures only name of this player is changed)
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            ChangeColor(Color.red);
        }
        ColorChanged();
        NameChanged();
    }
}
