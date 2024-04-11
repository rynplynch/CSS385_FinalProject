using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{


    public GameObject player;
    public Vector3 offset;

    private Rigidbody playerRB;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = player.transform.position + offset;

        transform.LookAt(playerRB.velocity + player.transform.position);

    }
}
