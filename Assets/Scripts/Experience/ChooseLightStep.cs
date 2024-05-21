using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChooseLightStep : MonoBehaviour
{
    public GameObject currentPanel;
    [SerializeField] private InputActionReference refAction; 
    public bool SnapTurn;
    [SerializeField] private Button buttonToUnlock; 

    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    public Toggle toggle4;
    public Toggle toggle5;
    public int maxValuer;
    private int valuer;

    private bool allChecked = false;
    // Start is called before the first frame update
    void Start()
    {
        refAction.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPanel.activeSelf == true){
            // Check if the snap turn button is pressed
            if (refAction.action.triggered)
            {
                 // Execute snap turn logic
                ActionPerformed();
            }
            
        }
    }

    private void ActionPerformed()
    {  
        // Read the vector2 value from the snap turn action
        Vector2 input = refAction.action.ReadValue<Vector2>();

        if (SnapTurn)
        {
             // Check the direction of the snap turn based on horizontal input
             float directionSide = input.x;
            Debug.Log("Snap Turn Direction: " + directionSide);
            if (directionSide > 0) // Snap turn right
            {
                valuer += 1;
            }
            else if (directionSide < 0) // Snap turn left
            {
                valuer -= 1;
        
            }
       
        }

        if(!SnapTurn)
        {
            // Check the direction of the snap turn based on vertical input
            float directionUp = input.y;
            Debug.Log("Snap Turn Direction: " + directionUp);
            if (directionUp > 0)
            {
                valuer += 1;
            }
            else if (directionUp < 0) 
            {
                valuer -= 1;
            }
        }


        if (valuer == maxValuer || valuer== -maxValuer) 
        {
            allChecked = true;
        }

        if (allChecked ){
            if (toggle1.isOn || toggle2.isOn || toggle3.isOn || toggle4.isOn || toggle5.isOn)
            {
                buttonToUnlock.interactable = true;
            }
        }
    }

}
