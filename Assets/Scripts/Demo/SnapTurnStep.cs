using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnapTurnStep : MonoBehaviour
{
    public GameObject currentPanel; // Reference to the panel you want to disappear
    public GameObject nextPanel; // Reference to the panel you want to appear   
    public GameObject zoneBlue;
    public GameObject teleportation;
    [SerializeField] public InputActionReference snapTurnAction; // Reference to the snap turn input action
    private bool turnLeft = false; 
    private bool turnRight = false;

     private void Start()
    {
        //currentPanel.SetActive(true); // Make sure the current panel is initially active

        // Enable the input action
        snapTurnAction.action.Enable();
    }

    private void Update()
    {
        if (currentPanel.activeSelf == true){
            // Check if the snap turn button is pressed
            if (snapTurnAction.action.triggered)
            {
                 // Execute snap turn logic
                OnSnapTurnPerformed();
            }
        }
        
    }

    // Called when the snap turn input action is performed
    private void OnSnapTurnPerformed()
    {  
        // Read the vector2 value from the snap turn action
        Vector2 input = snapTurnAction.action.ReadValue<Vector2>();

        // Check the direction of the snap turn based on horizontal input
        float direction = input.x;
        Debug.Log("Snap Turn Direction: " + direction);
        if (direction > 0) // Snap turn right
        {
            turnRight = true;
        }
        else if (direction < 0) // Snap turn left
        {
            turnLeft = true;
        }

        if (turnLeft && turnRight) // If the snap turn direction is 2
        {
            // Set the panel to inactive
            currentPanel.SetActive(false);
            // Set the next panel to active
            nextPanel.SetActive(true);
            // Set the zone to active
            zoneBlue.SetActive(true);
            teleportation.SetActive(true);
            
        }
    }
}
