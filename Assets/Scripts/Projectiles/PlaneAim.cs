using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAim : MonoBehaviour
{
    public Transform planeTransform;
    public float distance; // distance to see crosshair when raycast not hitting object
    public float maxDistance = 250f; // Raycast range limit
    public bool fixedAim = false;
    public bool raycastAim = true;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fixedAim == true && raycastAim == false)
        {
            FixedAim();
        }
        else if (fixedAim == false && raycastAim == true)
        {
            RayCastAim();
        }
        else if (fixedAim == true && raycastAim == true)
        {
            FixedAim();
            RayCastAim();
        }
        else{}
    }

    // Fixed aimed
    private void FixedAim()
    {
        
        transform.position = planeTransform.position + planeTransform.forward * distance;
    }

    // Raycast aim
    private void RayCastAim()
    {
        
        RaycastHit hit;
        // Cast a ray from the plane's position in the forward direction
        if (Physics.Raycast(planeTransform.position, planeTransform.forward, out hit, maxDistance))
        {
            // If the ray hits something, set the position of the crosshair to the point of aim
            transform.position = hit.point;
        }
        else
        {
            // If no hit within the maximum distance, set the position far in the forward direction of the plane
            transform.position = planeTransform.position + planeTransform.forward * distance;
        }
    }
}
