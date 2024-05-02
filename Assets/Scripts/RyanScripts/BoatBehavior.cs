using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatBehavior : MonoBehaviour
{
    private Transform cameraFirePoint;

    // used to manipulate the force applied to the player
    [Tooltip("Recommend settings\nAcceleration: [0-10]\nMaxThrust: [0-10]")]
    public float forwardSpeed;
    public float rotationalSpeed;
    private Vector3 rotationVectorR;
    private Vector3 rotationVectorL;
    public float maxThrustForce;
    public float maxRotationalForce;

    public InputAction forward;
    public InputAction backward;
    public InputAction right;
    public InputAction left;

    // used to apply force to the boat
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // cameraFirePoint = Camera.main.transform;
        forward.Enable();
        backward.Enable();
        left.Enable();
        right.Enable();

        // grab reference to attached rigid body component
        rb = this.GetComponent<Rigidbody>();

        // set how heavy the boat is
        rb.SetDensity(10);

    }

    private void MovePlayer(){
        float time = Time.deltaTime;

        // manipulate the current vector with another movement vector
        Vector3 newPos = this.transform.position;

        // construct rotation vector
        rotationVectorR = new Vector3(0, rotationalSpeed * time, 0);
        rotationVectorL = new Vector3(0, -rotationalSpeed * time, 0);

        // get the current rotation of the boat
        Quaternion currentRot = this.transform.rotation;

        if (forward.IsPressed()){
            // add in the forward movement vector
            // using the forward vector relative to itself
            newPos += this.transform.forward * time * forwardSpeed;
        }
        if (backward.IsPressed()){
            newPos += -this.transform.forward * time * forwardSpeed;
        }

        if (left.IsPressed()){
            // apply the left rotation vector to the current rotation
            rb.MoveRotation(currentRot * Quaternion.Euler(rotationVectorL));
        } else if (right.IsPressed()){
            rb.MoveRotation(currentRot * Quaternion.Euler(rotationVectorR));
        }

        rb.MovePosition(newPos);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

    }
}
