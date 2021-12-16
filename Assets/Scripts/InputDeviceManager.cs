using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputDeviceManager : MonoBehaviour
{
    public enum InputDevice
    {
        XR_LEFT_HAND = 1,
        XR_RIGHT_HAND = 2,
        XR_HEADSET = 4,
        KEYBOARD = 8,
        MOUSE = 16,
        GAMEPAD = 32
    };

    static InputDevice deviceConnections;

    public static UnityEvent<InputDevice> onDeviceConnected;

    public static UnityEvent<InputDevice> onDeviceDisconnected;

    Dictionary<string, InputDevice> deviceFromString;

    public static bool DeviceConnected(InputDevice device) { return (deviceConnections & device) != 0; }

    public static UnityEngine.InputSystem.InputDevice currentGnomeDevice;

    private void Awake()
    {
        if (onDeviceConnected == null)
            onDeviceConnected = new UnityEvent<InputDevice>();
        if (onDeviceDisconnected == null)
            onDeviceDisconnected = new UnityEvent<InputDevice>();
        

        deviceFromString.Add("XRI LeftHand", InputDevice.XR_LEFT_HAND);
        deviceFromString.Add("XRI RightHand", InputDevice.XR_RIGHT_HAND);
        deviceFromString.Add("XRI HMD", InputDevice.XR_HEADSET);
        deviceFromString.Add("Keyboard", InputDevice.KEYBOARD);
        deviceFromString.Add("Mouse", InputDevice.MOUSE);
        deviceFromString.Add("Gamepad", InputDevice.GAMEPAD);
    }

    void UpdateConnectedDevices()
    {
        deviceConnections = 0;
        foreach (var device in InputSystem.devices)
        {
            if (deviceFromString.TryGetValue(device.description.deviceClass, out InputDevice inputDevice))
            {
                deviceConnections |= inputDevice;
            }
        }
    }

    private void Start()
    {
        UpdateConnectedDevices();
    }

    private void Update()
    {
        UpdateConnectedDevices();
    }
}