using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] bool triggerOnce = false;
    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
        if (triggerOnce)
        {
            Destroy(this.gameObject);
        }
    }
}
