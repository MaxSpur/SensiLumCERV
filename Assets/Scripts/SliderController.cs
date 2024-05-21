using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SliderController : MonoBehaviour
{

    public TextMeshProUGUI sliderText;
    [SerializeField] private int  maxValuer = 10;

    public int id;

    public void SetSliderValue(float value)
    {
        
        float newValue = value * maxValuer;
        int newValueInt = (int)newValue;
        sliderText.text = newValueInt.ToString();

        if(id == 1){
            switch (sliderText.text){
                case "0":
                    sliderText.text = "Jamais";
                    break;
                case "1":
                    sliderText.text = "Jamais";
                    break;
                case "2":
                    sliderText.text = "Rarement";
                    break;
                case "3":
                    sliderText.text = "Rarement";
                    break;
                case "4":
                    sliderText.text = "Parfois";
                    break;
                case "5":
                    sliderText.text = "Parfois";
                    break;
                case "6":
                    sliderText.text = "Parfois";
                    break;
                case "7":
                    sliderText.text = "Souvent";
                    break;
                case "8":
                    sliderText.text = "Souvent";
                    break;
                case "9":
                    sliderText.text = "Tous les jours";
                    break;
                case "10":
                    sliderText.text = "Tous les jours";
                    break;
            }
        }
        
        
    }

    //Update is called once per frame
    void Update()
    {
        SetSliderValue(gameObject.GetComponent<UnityEngine.UI.Slider>().value);
    }
}
