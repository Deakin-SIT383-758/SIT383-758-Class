using System;
using System.Collections.Generic;

public interface IPlaneDataProvider
{
    IReadOnlyDictionary<string, PlaneData> ActivePlanes { get; }

    event Action<PlaneData> OnPlaneUpdated;
    event Action<string> OnPlaneRemoved;

    void Tick(float deltaTime);
}