using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBoat : MonoBehaviour
{
    public float thrustAcceleration;
    public float turnRate;
    public float rudderDecayRate;
    public float maxTurnRadius;

    public InputAction increaseThrust;
    public InputAction decreaseThrust;
    public InputAction rudderLeft;
    public InputAction rudderRight;
    public InputAction stopEngine;

    private float thrust;
    private float rudder = 1;
    private Rigidbody rb;
    private Vector3 appliedForce;

    // Start is called before the first frame update
    void Start()
    {
        increaseThrust.Enable();
        decreaseThrust.Enable();
        rudderLeft.Enable();
        rudderRight.Enable();
        stopEngine.Enable();

        rb = gameObject.GetComponent<Rigidbody>();
        thrust = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopEngine.WasPerformedThisFrame())
        {
            thrust = 0;
        }

        if (increaseThrust.IsPressed())
        {
            thrust += thrustAcceleration;
        }

        if (decreaseThrust.IsPressed())
        {
            thrust -= thrustAcceleration;
        }

        // if the rudder left action button is pressed
        // and the rudders value is greater than -1
        if (rudderLeft.IsPressed() & rudder > -maxTurnRadius)
        {
            // decrease the value of rudder
            rudder = rudder - turnRate;
        }
        // if the rudder right action button is pressed
        // and the rudders value is less than 1
        else if (rudderRight.IsPressed() & rudder < maxTurnRadius)
        {
            // increase value of rudder
            rudder = rudder + turnRate;
        }
        else
        {
            // otherwise the rudder returns slowly to 0
            if (rudder < 0)
            {
                // adding if rudder is negative
                rudder += rudderDecayRate;
            }
            else if (rudder > 0)
            {
                // subtracting if rudder is positive
                rudder -= rudderDecayRate;
            }
        }

        // solve for the new appliedForce vector
        // the value of rudder can only be between -1 and 1
        // z component equals (1 - |rudder|) * thrust
        float zComp = (1 - Mathf.Abs(rudder)) * thrust;

        // x component equals rudder * thrust
        float xComp = Mathf.Abs(rudder) * thrust;

        // the sign of the rudder tells us the direction of the x component
        // - is equal to turning left
        // + is equal to turning right
        if (rudder < 0)
        {
            xComp = -xComp;
        }

        // set the thrust vector using the solved values
        appliedForce.Set(xComp, 0, zComp);

        rb.AddRelativeForce(appliedForce * Time.deltaTime);

        Debug.Log(appliedForce);
    }
}
