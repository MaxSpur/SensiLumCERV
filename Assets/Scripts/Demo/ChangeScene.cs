using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour
{
    

    [SerializeField] private string sceneName;

     // Méthode appelée lorsque le bouton est pressé
    public void SwitchScene(string sceneName)
    {
        // Charger la nouvelle scène en utilisant son nom
        SceneManager.LoadScene(sceneName);
    }

}
