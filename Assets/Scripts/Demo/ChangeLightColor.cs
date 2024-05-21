using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColor : MonoBehaviour
{
     public TMPro.TextMeshProUGUI text; // Reference to the TextMeshProUGUI component
     public Light lightToControl; // Reference to the Light component

    void Update()
    {
        // Check if the serialized text matches certain value
        if (text.text == "Passereau" || text.text == "Ciel Étoilé")
        {
            lightToControl.color = Color.black;
        }
        else if (text.text == "Humain" || text.text == "Éclairage Rouge")
        {
            lightToControl.color = Color.red;
        }
        else if (text.text == "Anoure" || text.text == "Éclairage LED")
        {
            lightToControl.color = Color.yellow;
        }
        else if (text.text == "Chauve-Souris" || text.text == "Éclairage Standard")
        {
            lightToControl.color = Color.white;
        }
        else if (text.text == "Pleine Lune")
        {
            lightToControl.color = Color.blue;
        }
        else
        {
            Debug.Log("No color found for this species");
        }
    }
}
