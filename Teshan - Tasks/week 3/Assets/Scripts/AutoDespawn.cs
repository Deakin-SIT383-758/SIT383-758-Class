using UnityEngine;
using Fusion;

public class AutoDespawn : NetworkBehaviour
{
    public float lifetime = 4f;

    private float timer;

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        timer += Runner.DeltaTime;

        if (timer >= lifetime)
        {
            Runner.Despawn(Object);
        }
    }
}