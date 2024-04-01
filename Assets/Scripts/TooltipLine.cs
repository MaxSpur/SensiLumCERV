using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TooltipLine : MonoBehaviour
{
    public Transform basePosition;
    public Transform targetPosition;

    private LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, basePosition.position);
        lineRenderer.SetPosition(1, targetPosition.position);
    }
}
