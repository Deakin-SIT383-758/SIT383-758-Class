using System;
using UnityEngine;

public enum PlaneStatus { Normal, Emergency, CommFailure, Hijack }

[Serializable]
public class PlaneData
{
    #region Identity
    public string hex;
    public string flight;
    #endregion

    #region Position
    public float lat;
    public float lon;
    public float ASL;
    public Vector3 mapPosition;
    #endregion

    #region Movement
    public float gs;
    public float track;
    public float baroRate;
    #endregion

    #region Status
    public string squawk;
    public PlaneStatus status;
    #endregion

    #region Timing
    public DateTimeOffset lastSeen;
    public float seenPos;
    #endregion

    //Reads the baroRate variable to determine whether the aircraft is ascending, descending or staying level
    public string VerticalTrend =>
        baroRate > 100f  ? "upwards" :
        baroRate < -100f ? "downwards" : "level";

    public bool IsStale(float thresholdSeconds = 30f) => seenPos > thresholdSeconds;

    //catch a squawk code and translate it into the appropriate plane status,
    //If any other squawk code is entered, check the emergency serialized field.
    //If it's not set to 'null', raise an emergency anyways.
    public static PlaneStatus StatusFromSquawk(string squawk, string emergency) =>
        squawk switch
        {
            "7700" => PlaneStatus.Emergency,
            "7600" => PlaneStatus.CommFailure,
            "7500" => PlaneStatus.Hijack,
            _ => emergency != null && emergency != "none" 
                ? PlaneStatus.Emergency
                : PlaneStatus.Normal,
        };

    //Deserialize fetched data from the json API and store it for future use
    public static PlaneData FromJson(long now, PlaneDataJson json)
    {
        return new PlaneData
        {
            hex        = json.hex,
            flight     = json.flight?.Trim() ?? json.hex,
            lat        = json.lat,
            lon        = json.lon,
            ASL        = json.alt_baro_value,
            gs         = json.gs,
            track      = json.track,
            baroRate   = json.baro_rate,
            squawk     = json.squawk ?? "0000",
            status     = StatusFromSquawk(json.squawk, json.emergency),
            lastSeen   = DateTimeOffset.FromUnixTimeSeconds(now - (long)json.seen),
            seenPos    = json.seen_pos,
        };
    }
}

[Serializable]
public class PlaneDataJson
{
    public string hex;
    public string flight;
    public float lat;
    public float lon;
    public float alt_baro_value;
    public float gs;
    public float track;
    public float baro_rate;
    public float seen_pos;
    public float seen;
    public string squawk;
    public string emergency;
}


