using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SliderController : MonoBehaviour
{

    public TextMeshProUGUI sliderText;
    [SerializeField] private int  maxValuer = 10;

    public void SetSliderValue(float value)
    {
        
        float newValue = value * maxValuer;
        int newValueInt = (int)newValue;
        sliderText.text = newValueInt.ToString();
        
    }

    //Update is called once per frame
    void Update()
    {
        SetSliderValue(gameObject.GetComponent<UnityEngine.UI.Slider>().value);
    }
}
