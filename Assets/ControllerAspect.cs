using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerAspect : MonoBehaviour
{
    public GameObject controller;
    public Material controllerMaterial;
    public Material unlitMaterial;

    public TMP_Text lightmap_text;

    // Start is called before the first frame update
    void Start()
    {
        controllerMaterial = controller.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (lightmap_text.text == "Ciel Étoilé"){
            controller.GetComponent<Renderer>().material = unlitMaterial;
        }
        else{
            controller.GetComponent<Renderer>().material = controllerMaterial;
        }
    }
}
