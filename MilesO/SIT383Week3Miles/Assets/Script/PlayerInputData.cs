using Fusion;
using System;

public struct PlayerInputData : INetworkInput
{
    public float horizontal;
    public float vertical;
    public bool spawn;
    public bool spawnPressed;
}