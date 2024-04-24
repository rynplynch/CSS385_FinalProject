using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{

    public float mouseSensitivity = 200f;
    public Transform playerBody;
    public Rigidbody playerBodyRigid;
    public Vector2 mouseMove;
    float xRotation = 0f;
    float yRotation = 0f;
    public bool cursorLock;
    public bool perspective;

    [SerializeField]
    private float turnMagnitude;

    // Update is called once per frame
    void Update()
    {
        //Gets mouse input
        mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //Increases the sensitivity to movement
        mouseMove *= mouseSensitivity * Time.deltaTime;

        //Clamps the value of the mouse movement if above a certain magnitude
        if (mouseMove.magnitude > turnMagnitude)
        {
            mouseMove.Normalize();
            mouseMove *= turnMagnitude;
        }

        //Converts the movement into rotation for the quaternion
        xRotation -= mouseMove.y;
        yRotation += mouseMove.x;

        //Moves the sphere to the player
        transform.position = playerBody.position;

        //Rotates the sphere
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.Rotate(Vector3.up * mouseMove.x);
        playerBody.Rotate(Vector3.right * mouseMove.y);
    }
}
