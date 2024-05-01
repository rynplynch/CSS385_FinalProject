using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera3D : MonoBehaviour
{
    public Transform player;
    public float cameraDistance;
    public float cameraHeight;

    void LateUpdate()
    {
        if (player != null)
        {
            // set camera position equal to players forward position
            transform.position = player.transform.position - player.transform.forward * cameraDistance;

            // change camera height in relation to player
            transform.position += new Vector3(0, cameraHeight, 0);

            transform.LookAt(player.transform);
        }
    }
}
