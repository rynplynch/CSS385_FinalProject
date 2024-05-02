using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera3D : MonoBehaviour
{
    public float cameraDistance;
    public float cameraHeight;
    private GameObject player;
    public static FollowCamera3D Instance { get; private set; }

    void LateUpdate()
    {
<<<<<<< HEAD
        if (player){
        // set camera position equal to players forward position
        transform.position = player.transform.position - player.transform.forward * cameraDistance;
=======
        if (player != null)
        {
            // set camera position equal to players forward position
            transform.position = player.transform.position - player.transform.forward * cameraDistance;
>>>>>>> ryan

            // change camera height in relation to player
            transform.position += new Vector3(0, cameraHeight, 0);

<<<<<<< HEAD
        transform.LookAt(player.transform);
=======
            transform.LookAt(player.transform);
>>>>>>> ryan
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void setFollowedPlayer(GameObject p) => player = p;

    public GameObject getFollowedPlayer() => player;
}
