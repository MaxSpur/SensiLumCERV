using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ToggleGroupController : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI labelText;

    void Start()
    {
        // Call the function once at the start to set initial label
        UpdateSelectedToggleLabel();
    }

    public void OnToggleChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            labelText.text = toggle.GetComponentInChildren<Text>().text;
        }
    }

    void Update()
    {
        // Call the function every frame to update the label
        UpdateSelectedToggleLabel();
    }

    void UpdateSelectedToggleLabel()
    {
        // Find the selected Toggle in the group
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        
        // If a Toggle is selected, update the label text
        if (selectedToggle != null)
        {
            labelText.text = selectedToggle.GetComponentInChildren<Text>().text;
        }
        else
        {
            labelText.text = "None"; // No toggle selected
        }
    }
}
