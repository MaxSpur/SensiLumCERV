using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureUpdate : MonoBehaviour
{
    public RawImage rawImage;
    public Texture Texture1; // Assign your Passereau texture in the Unity Editor
    public Texture Texture2; // Assign your Passereau texture in the Unity Editor
    public Texture Texture3; // Assign your Passereau texture in the Unity Editor
    public Texture Texture4; // Assign your Passereau texture in the Unity Editor
     public Texture Texture5; // Assign your Passereau texture in the Unity Editor

    // Serialized text value
    public TMPro.TextMeshProUGUI text; // Reference to the TextMeshProUGUI component

    void Update()
    {
        // Check if the serialized text matches certain value
        if (text.text == "Passereau" || text.text == "Ciel Étoilé")
        {
            rawImage.texture = Texture1;
        }
        else if (text.text == "Humain" || text.text == "Éclairage Rouge")
        {
            rawImage.texture = Texture2;
        }
        else if (text.text == "Grenouille" ||text.text == "Anoure" || text.text == "Éclairage LED")
        {
            rawImage.texture = Texture3;
        }
        else if (text.text == "Chauve-Souris" || text.text == "Éclairage Standard")
        {
            rawImage.texture = Texture4;
        }
        else if (text.text == "Pleine Lune")
        {
            rawImage.texture = Texture5;
        }
        else
        {
            Debug.Log("No texture found for this species");
        }
    }
}
