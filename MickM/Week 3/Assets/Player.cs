using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    public override void Spawned()
    {
        if (HasInputAuthority == false)
            return;

        transform.localScale = Random.Range(0.5f, 1.5f) * Vector3.one;

        var renderer = gameObject.GetComponentInChildren<Renderer>();
        renderer.material.SetColor("_BaseColor", Color.red);
    }
    
}
