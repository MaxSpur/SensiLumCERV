using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target; // Reference to the VR camera/headset
    public float smoothSpeed = 5f; // Smoothing speed
    public float followDelay = 0.1f; // Delay in following the target

    public float distance = 5; // Distance between the UI and the target

    private Vector3 offset; // Offset between UI and target

    void Start()
    {
        // Calculate initial offset
        offset = transform.position - target.position;
    }


    void LateUpdate()
    {
            Vector3 forwardDirection = target.forward;
            forwardDirection.y = 0;

            // Calculate the target position
            Vector3 targetPosition = target.transform.position + (forwardDirection.normalized * distance) ;

            // Smoothly move towards the target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Set the canvas position to the smoothed position
            transform.position = smoothedPosition;

            // Make the canvas face the target
            transform.LookAt(target);

    }
}
