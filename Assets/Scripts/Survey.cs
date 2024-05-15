using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Survey : MonoBehaviour
{
     [SerializeField] TextMeshProUGUI feedback1;
     [SerializeField] TextMeshProUGUI feedback2;
    [SerializeField] TextMeshProUGUI feedback3;

    [SerializeField] TextMeshProUGUI feedback4;
     [SerializeField] TextMeshProUGUI feedback5;

     [SerializeField] TextMeshProUGUI feedback6;
     [SerializeField] TextMeshProUGUI feedback7;

     [SerializeField] TextMeshProUGUI feedback8;

    // QUESTION 9 --------------------------------
     [SerializeField] TextMeshProUGUI feedback9;
     [SerializeField] TextMeshProUGUI feedback10;
     [SerializeField] TextMeshProUGUI feedback11;
     [SerializeField] TextMeshProUGUI feedback12;
     [SerializeField] TextMeshProUGUI feedback13;

    // URL of the Google Form - formResponse
    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdEXNLjUHKu7NjXHC_U8kDB01iu2Kl9F-D5VKHnWyUjD5H0sA/formResponse";
     WWWForm form;

    void Start()
    {
        form = new WWWForm();
    }

    
    public void SendFeedback1()
    {
        form.AddField("entry.741890070", feedback1.text);
    }

    public void SendFeedback2()
    {
       form.AddField("entry.1027838383", feedback2.text);
    }

    public void SendFeedback3()
    {
       form.AddField("entry.1010778585", feedback3.text);
    }

    public void SendFeedback4()
    {
       form.AddField("entry.1990389215", feedback4.text);
    }

    public void SendFeedback5()
    {
       form.AddField("entry.827258166", feedback5.text);
    }

    public void SendFeedback6()
    {
       form.AddField("entry.50045178", feedback6.text);
    }

     public void SendFeedback7()
    {
       form.AddField("entry.741890070", feedback7.text);
    }

     public void SendFeedback8()
    {
       form.AddField("entry.1681014148", feedback8.text);
    }

     public void SendFeedback9()
    {
       form.AddField("entry.1451158834", feedback9.text); // Réalisme
       form.AddField("entry.1846966001", feedback10.text);// Immersion
       form.AddField("entry.2058740240", feedback11.text);//Intuitif
       form.AddField("entry.1826083030", feedback12.text);//Fluidité
       form.AddField("entry.673368168", feedback13.text);//Ludique

    }

    public void SendForm()
    {
        //StartCoroutine(Post(feedback2.text,"entry.1027838383")); // Feedback 3 ID
        StartCoroutine(Post());
    }


    IEnumerator Post()
    {
        
        //form.AddField(fieldID, feedback);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();
    }
}
