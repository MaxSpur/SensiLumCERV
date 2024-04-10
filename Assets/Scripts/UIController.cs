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

     private Coroutine appearanceCoroutine;

    private void Start()
    {
        // Make the GameObject initially invisible when the scene launches
        gameObject.SetActive(false);
    }

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
        // Check if the input action comes from the right controller joystick
        if (context.action.name.Contains("Snap Turn") || context.action.name.Contains("Teleport Select") )
        {
            // If an appearance coroutine is already running, stop it
            if (appearanceCoroutine != null)
                StopCoroutine(appearanceCoroutine);

            // Set the object active and start the appearance coroutine
            gameObject.SetActive(true);
            appearanceCoroutine = StartCoroutine(DisappearAfterDelay());
        }
        else
        {
            // Toggle object visibility normally
            gameObject.SetActive(!gameObject.activeSelf);
        }
        OnToggleMenu?.Invoke();
        Debug.Log(context.action.name + " performed!");
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


    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Wait for 5 seconds
        gameObject.SetActive(false); // Deactivate the object after the delay
    }
}
