using UnityEngine;

public class PlaneMovement : Vehicle
{

    //public GameObject mouseTarget;

    public float thrust;
    public Vector2 wasdInput;
    //public float verticalInput;
    public Vector2 mouseMove;


    public float pitchSpeed;
    public float rollSpeed;
    public float yawSpeed;

    public float mouseSensitivity;
    [SerializeField]
    private float turnMagnitude;

    public float drag;
    /*
    public float pitchDrag;
    public float rollDrag;
    public float yawDrag;
    */

    public Rigidbody rb;

    public float timer;

    public bool isBoat;

    public float boatThrust;

    float xRotation = 0f;
    float yRotation = 0f;
    float zRotation = 0f;

    private Vector3 oldVelocity;


    // Start is called before the first frame update
    void Start()
    {
        //planeShoot = GetComponent<PlaneShoot>();

        //Sets player rigidbody
        rb = gameObject.GetComponent<Rigidbody>();
        //Locks the cursor so it doesn't go off screen
        //Can be commented out
        Cursor.lockState = CursorLockMode.Locked;



    }

    // Update is called once per frame
    void Update()
    {
        //Gets horizontal and vertical input
        wasdInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

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

        //Gets mouse movement, only useful for the realistic controls
        //mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //mouseMove.Normalize();

        float deltaTime = Time.deltaTime;

        timer += deltaTime;

        //mouseMove.x = 0;

        //Resets the player to the origin
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
        }

        //Debug to get the magnitude of the velocity
        /*
        if (timer % 1 - deltaTime < 0 && timer % 1 + deltaTime > 0)
        {
            Debug.Log(rb.velocity.magnitude);
            //timer = 0;
        }
        */

        //I just wanted to switch between the two easily
        if (isBoat)
        {
            BoatMove();
        }
        else
        {
            PlaneMove();
        }

        //Draws the direction of velocity for the vehicle
        Debug.DrawLine(transform.position, transform.position + rb.velocity, Color.cyan);
    }


    void PlaneMove()
    {
        //Force/Torque method
        float deltaTime = Time.deltaTime;

        //This clamps the input from the player
        float clampedVerticalInput = wasdInput.y;
        //Limits the value from 0-1 to 0-0.5
        clampedVerticalInput /= 2;
        //Changes the range to 0.5-1
        clampedVerticalInput += 0.5f;

        //This is the thrust force imparted by the engines
        rb.AddForce(transform.forward * deltaTime * (clampedVerticalInput * thrust));
        //This is the drag force from the air
        rb.AddForce(-rb.velocity * deltaTime * rb.velocity.magnitude * rb.velocity.magnitude * drag);

        //Makes the plane look at the target
        //transform.LookAt(mouseTarget.transform);

        //Just debug lines that show the direction of the transform directions
        Debug.DrawLine(transform.forward, transform.forward * 10, Color.green);
        Debug.DrawLine(transform.up, transform.up * 10, Color.blue);
        Debug.DrawLine(transform.right, transform.right * 10, Color.red);

        //Converts the movement into rotation for the quaternion
        xRotation -= mouseMove.y;
        yRotation += mouseMove.x;


        zRotation -= wasdInput.x;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        //transform.Rotate(Vector3.up * mouseMove.x);
        //transform.Rotate(Vector3.right * mouseMove.y);
        //transform.Rotate(Vector3.forward * wasdInput.x);

        //rb.velocity = (rb.velocity * 0.2f) + (oldVelocity * 0.8f);







        //oldVelocity = rb.velocity;

        //I still want to experiment with this but it's not really important

        /*
        //transform.forward = mouseTarget.transform.forward;
        //transform.TransformDirection(mouseTarget.transform);


        if (velocityNormalized != transform.forward)
        {
            //Point plane velocity in the transform.forward direction
        }
        rb.AddTorque(Vector3.forward * deltaTime * horizontalInput * rollSpeed);
        //rb.AddTorque(-Vector3.forward * deltaTime * rollDrag * rb.angularVelocity.z);


        rb.AddTorque(Vector3.right * deltaTime * mouseMove.y * pitchSpeed);
        //rb.AddTorque(-Vector3.right * deltaTime * pitchDrag * rb.angularVelocity.x);


        //rb.AddTorque(Vector3.forward * Time.deltaTime * (mouseMove.x * yawSpeed - yawDrag * rb.angularVelocity.y));
        rb.AddTorque(Vector3.up * deltaTime * mouseMove.x * yawSpeed);
        //rb.AddTorque(-Vector3.up * deltaTime * yawDrag * rb.angularVelocity.y);


        if(transform.rotation != Quaternion.identity && mouseMove == Vector2.zero && horizontalInput == 0)
        {
            //transform.rotation = Quaternion.identity;
        }

        //Transform method
        //transform.Translate(Vector3.forward * thrust * Time.deltaTime * verticalInput);
        //transform.Rotate(Vector3.forward * Time.deltaTime * mouseX * rollSpeed);
        //transform.Rotate(Vector3.right * Time.deltaTime * mouseY * pitchSpeed);
        //transform.Rotate(Vector3.up * Time.deltaTime * horizontalInput * yawSpeed);
        */
    }
    void BoatMove()
    {
        float deltaTime = Time.deltaTime;

        rb.AddForce(transform.forward * deltaTime * boatThrust * wasdInput.y * 100000);
        //rb.AddForce(-transform.forward * rb.velocity);

        rb.AddTorque(transform.up * deltaTime * boatThrust * wasdInput.x);

    }
}
