using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    public GameObject player;

    public Vector2 mouseMove;

    public Vector3 targetOffset;

    public GameObject test;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //mouseMove.Normalize();

        //mouseMove *= 5;

        //Clamp Max Mouse movements



        if(mouseMove.x > 10)
        {
            mouseMove.x = 10;
        }
        if(mouseMove.x < -10)
        {
            mouseMove.x = -10;
        }
        if(mouseMove.y > 10)
        {
            mouseMove.y = 10;
        }
        if(mouseMove.y < -10)
        {
            mouseMove.y = -10;
        }

        //Vector3 lastPos = transform.position;


        float targetOffsetMagnitude = targetOffset.magnitude;

        targetOffset = transform.forward;
        
        
        
        targetOffset *= targetOffsetMagnitude;

        test.transform.position = targetOffset;

        Vector3 currentPos = transform.position;
        Vector3 desiredPos = new Vector3(mouseMove.x + player.transform.position.x + targetOffset.x, mouseMove.y + player.transform.position.y + targetOffset.y, player.transform.position.z + targetOffset.z);




        //desiredPos *= player.transform.forward;


        transform.position = desiredPos;




    }
}
