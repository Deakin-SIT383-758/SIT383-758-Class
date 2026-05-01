using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public static class HapticsManager
{
    public static void Pulse(XRNode hand, float amplitude = 0.5f, float duration = 0.1f)
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(hand, devices);
        foreach (var device in devices)
            device.SendHapticImpulse(0, Mathf.Clamp01(amplitude), duration);
    }

    public static void PulseBoth(float amplitude = 0.5f, float duration = 0.1f)
    {
        Pulse(XRNode.LeftHand, amplitude, duration);
        Pulse(XRNode.RightHand, amplitude, duration);
    }
}
