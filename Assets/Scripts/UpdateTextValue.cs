using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTextValue : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text; // Reference to the TextMeshProUGUI component
    public TMPro.TextMeshProUGUI RefText; // Reference to the TextMeshProUGUI component

    // Update is called once per frame
    void Update()
    {
        text.text = RefText.text;
    }
}
