using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationRestriction : MonoBehaviour
{
    public LayerMask teleportRestriction;
    public bool isRestricted = false;

    void Start()
    {
        if (GetComponent<UnityEngine.XR.Interaction.Toolkit.TeleportationArea>() == null)
        {
            Debug.LogError("TeleportationRestriction script must be attached to a GameObject with a TeleportationArea component.");
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((teleportRestriction & (1 << other.gameObject.layer)) != 0)
        {
            isRestricted = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((teleportRestriction & (1 << other.gameObject.layer)) != 0)
        {
            isRestricted = false;
        }
    }
}
