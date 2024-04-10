using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Survey : MonoBehaviour
{
     [SerializeField] TextMeshProUGUI feedback1;

    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdEXNLjUHKu7NjXHC_U8kDB01iu2Kl9F-D5VKHnWyUjD5H0sA/formResponse";

    
    public void Send()
    {
        StartCoroutine(Post(feedback1.text));
    }

    IEnumerator Post(string s1)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.741890070", s1);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        
        yield return www.SendWebRequest();

    }
}
