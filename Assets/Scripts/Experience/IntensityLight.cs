using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntensityLight : MonoBehaviour
{
    public Light light;
   public TextMeshProUGUI sliderText;
    [SerializeField] private int  maxValuer = 10;

   private int intensityValue = 0;

    // Update is called once per frame
    void Update()
    {
       IntensityValue(gameObject.GetComponent<UnityEngine.UI.Slider>().value);
    }

    private void IntensityValue(float value)
    {
        float newValue = value * maxValuer;
        int newValueInt = (int)newValue;
        sliderText.text = newValueInt.ToString();
        
        light.intensity = newValueInt;
    }
}
