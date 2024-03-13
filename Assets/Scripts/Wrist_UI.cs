using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Wrist_UI : MonoBehaviour
{
    public InputActionAsset _Inputactions;

    private Canvas WristUICanva;
    public InputActionReference Menu;

    // Start is called before the first frame update
    void Start()
    {
        WristUICanva = GetComponent<Canvas>();
        //Menu = _Inputactions.FindActionMap("XRI LeftHand").FindAction("Menu");
        Menu.action.Enable();
        Menu.action.performed += ToggleMenu;
    }

    private void Update()
    {
        //Menu.performed += ToggleMenu;
        if (Menu.action.triggered)
            Debug.Log("coucou ");
    }

    private void OnDestroy()
    {
        Menu.action.performed -= ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        WristUICanva.GetComponent<GameObject>().SetActive(true);
        Debug.LogError("Pressed");
    }
}
