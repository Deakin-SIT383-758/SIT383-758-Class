using UnityEngine;
using Fusion;

public class PlayerTrailSpawner : NetworkBehaviour
{
    public GameObject trailPrefab;
    public float spawnEverySeconds = 0.4f;
    public float trailLifetime = 4f;

    private float timer;

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        timer += Runner.DeltaTime;

        if (timer >= spawnEverySeconds)
        {
            timer = 0f;

            Vector3 spawnPos = transform.position;
            spawnPos.y = 0.05f;

            NetworkObject trail = Runner.Spawn(
                trailPrefab,
                spawnPos,
                Quaternion.identity
            );
        }
    }
}