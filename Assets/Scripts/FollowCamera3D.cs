using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera3D : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }
}
