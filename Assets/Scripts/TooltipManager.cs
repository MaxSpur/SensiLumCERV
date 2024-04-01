using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{
     private void OnEnable()
    {
        // Subscribe to the toggle menu event
        UIController.OnToggleMenu += ToggleTooltips;
        InputSystem.onDeviceChange += OnDeviceChange;
        Debug.Log("TooltipManager: Enabled");
    }

    private void OnDisable()
    {
        // Unsubscribe from the toggle menu event
        UIController.OnToggleMenu -= ToggleTooltips;
        InputSystem.onDeviceChange -= OnDeviceChange;
        Debug.Log("TooltipManager: Disabled");
    }

    // Function to toggle tooltips
    private void ToggleTooltips()
    {
        // Implement your tooltip toggling logic here
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                UIController.OnToggleMenu -= ToggleTooltips;
                break;
            case InputDeviceChange.Reconnected: 
            
                UIController.OnToggleMenu += ToggleTooltips;
                break;
        }
    }
}
