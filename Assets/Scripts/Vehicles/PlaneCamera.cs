using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneCamera : MonoBehaviour
{
    private GameLogic gCtrl;

    public float cameraDistance;
    public float cameraHeight;
    public bool isFollowingMouse = false;

    public GameObject player;

    void LateUpdate()
    {
        // Follow mouse for boat or default for either
        if (player != null)
        {
            transform.position = player.transform.position - player.transform.forward * cameraDistance + Vector3.up * cameraHeight;
            transform.LookAt(player.transform);
        }
    }

    private void Start()
    {
    }

    public void setFollowedPlayer(GameObject p) => player = p;
    public GameObject getFollowedPlayer() => player;
}
