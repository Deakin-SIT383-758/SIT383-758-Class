using System;
using System.Collections.Generic;
using UnityEngine;

public class MockPlaneDataProvider : MonoBehaviour, IPlaneDataProvider
{
    [SerializeField] private List<MockFlightPlan> flightPlans = new();

    private readonly Dictionary<string, PlaneData> _planes = new();
    private readonly Dictionary<string, FlightState> _states = new();

    #region IPlaneDataProvider
    public IReadOnlyDictionary<string, PlaneData> ActivePlanes => _planes;
    public event Action<PlaneData> OnPlaneUpdated;
    public event Action<string> OnPlaneRemoved;
    #endregion

    private class FlightState
    {
        public MockFlightPlan plan;
        public int waypointIndex;
        public float segmentT;
        public float speedJitter;
    }

    private void Start() => InitializePlanes();

    private void InitializePlanes()
    {
        foreach (var plan in flightPlans)
        {
            if (plan == null || plan.waypoints.Count < 2) continue;

            string hex = GenerateMockHex();

            var plane = new PlaneData
            {
                hex         = hex,
                flight      = plan.callsign,
                lat         = plan.waypoints[0].z,
                lon         = plan.waypoints[0].x,
                ASL         = plan.cruiseAltitude,
                gs          = plan.cruiseSpeed,
                track       = 0f,
                baroRate    = 0f,
                squawk      = "1200",
                status      = PlaneStatus.Normal,
                lastSeen    = DateTimeOffset.UtcNow,
                seenPos     = 0f,
                mapPosition = plan.waypoints[0],
            };

            _planes[hex] = plane;
            _states[hex] = new FlightState
            {
                plan          = plan,
                waypointIndex = 0,
                segmentT      = 0f,
                speedJitter   = UnityEngine.Random.Range(-15f, 15f),
            };
        }
    }

    private void Update() => Tick(Time.deltaTime);

    public void Tick(float deltaTime)
    {
        foreach (var (id, plane) in _planes)
        {
            StepPlane(plane, _states[id], deltaTime);
            plane.lastSeen = DateTimeOffset.UtcNow;
            plane.seenPos = 0f;
            OnPlaneUpdated?.Invoke(plane);
        }
    }

    #region Simulation

    private void StepPlane(PlaneData plane, FlightState state, float deltaTime)
    {
        var waypoints = state.plan.waypoints;
        int lastIdx = waypoints.Count - 1;

        if (state.waypointIndex >= lastIdx) return;

        float effectiveSpeed = Mathf.Max(
            state.plan.cruiseSpeed
                + state.speedJitter
                + Mathf.Sin(Time.time * 0.35f + state.speedJitter * 0.1f) * 4f,
            30f
        );
        plane.gs = effectiveSpeed;

        Vector3 from = waypoints[state.waypointIndex];
        Vector3 to   = waypoints[state.waypointIndex + 1];
        float segLen = Vector3.Distance(from, to);

        if (segLen > 0.001f)
            state.segmentT += (effectiveSpeed * deltaTime) / segLen;

        while (state.segmentT >= 1f && state.waypointIndex < lastIdx)
        {
            state.segmentT -= 1f;
            state.waypointIndex++;
            if (state.waypointIndex < lastIdx)
            {
                from   = waypoints[state.waypointIndex];
                to     = waypoints[state.waypointIndex + 1];
            }
        }

        if (state.waypointIndex >= lastIdx)
        {
            plane.mapPosition = waypoints[lastIdx];
            return;
        }

        from = waypoints[state.waypointIndex];
        to   = waypoints[state.waypointIndex + 1];

        plane.mapPosition = Vector3.Lerp(from, to, Mathf.Clamp01(state.segmentT));
        plane.lat = plane.mapPosition.z;
        plane.lon = plane.mapPosition.x;
        plane.ASL = plane.mapPosition.y;

        Vector3 dir = to - from;
        if (dir.sqrMagnitude > 0.0001f)
            plane.track = ((Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg) + 360f) % 360f;

        float prevAlt = from.y + (to.y - from.y) * Mathf.Clamp01(state.segmentT - 0.01f);
        plane.baroRate = (plane.ASL - prevAlt) / Mathf.Max(deltaTime, 0.001f) * 60f;
    }

    private static string GenerateMockHex()
    {
        // Australian ICAO range starts at 7C0000
        int value = UnityEngine.Random.Range(0x7C0000, 0x7CFFFF);
        return value.ToString("X6");
    }

    #endregion
}

