using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject gameObject;
    public delegate void ToggleMenuDelegate();
    public static event ToggleMenuDelegate OnToggleMenu;
    public InputActionReference inputActionReference;

    private void Awake()
    {
        inputActionReference.action.Enable();
        inputActionReference.action.performed += ToggleMenu;
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDestroy()
    {
        inputActionReference.action.Disable();
        inputActionReference.action.performed -= ToggleMenu;
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        OnToggleMenu?.Invoke();
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                inputActionReference.action.Disable();
                inputActionReference.action.performed -= ToggleMenu;
                break;
            case InputDeviceChange.Reconnected: 
                inputActionReference.action.Enable();
                inputActionReference.action.performed += ToggleMenu;
                break;
        }
    }

}
