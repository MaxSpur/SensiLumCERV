using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recenter : MonoBehaviour
{
    // Reference to the VR camera rig or the GameObject representing the player's head
    public Transform vrHead;

    // Function to recenter the user's position
    public void RecenterStanding()
    {
        Vector3 recenterPosition = new Vector3(0f, 0f, 0f);
        // Get the current position of the VR camera rig or head
        Vector3 currentPosition = vrHead.position;

        // Calculate the position offset for recentering
        Vector3 positionOffset = recenterPosition - currentPosition;

        // Apply the position offset to recenter the user
        transform.position += positionOffset;
    }

    
    // Function to recenter the user's position
    public void RecenterSitting()
    {
        Vector3 recenterPosition = new Vector3(0f, 0f, 0f);

        vrHead.position = recenterPosition;
        // // Get the current position of the VR camera rig or head
        // Vector3 currentPosition = vrHead.position;

        // // Calculate the position offset for recentering
        // Vector3 positionOffset = recenterPosition - currentPosition;

        // // Apply the position offset to recenter the user
        // transform.position += positionOffset;
    }

}
