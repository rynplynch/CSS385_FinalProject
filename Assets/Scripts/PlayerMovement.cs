using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public GameObject mouseTarget;


    public float thrust;
    public float horizontalInput;
    public float verticalInput;
    public Vector2 mouseMove;


    public float pitchSpeed;
    public float rollSpeed;
    public float yawSpeed;

    
    public float drag;
    /*
    public float pitchDrag;
    public float rollDrag;
    public float yawDrag;
    */

    public Rigidbody rb;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        float deltaTime = Time.deltaTime;

        timer += deltaTime;

        //mouseMove.x = 0;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
        }

        //Vector2 mouseMove = new Vector2(mouseX, mouseY);

        mouseMove.Normalize();

        if (timer % 1 - deltaTime < 0 && timer % 1 + deltaTime > 0)
        {
            Debug.Log(rb.velocity.magnitude);
            //timer = 0;
        }

        Move(deltaTime);
    }


    void Move(float deltaTime)
    {
        //Force/Torque method

        float clampedVerticalInput = verticalInput;

        clampedVerticalInput /= 2;
        clampedVerticalInput += 0.5f;

        rb.AddForce(transform.forward * deltaTime * (clampedVerticalInput * thrust));
        rb.AddForce(-rb.velocity * deltaTime * rb.velocity.magnitude * rb.velocity.magnitude * drag);


        Vector3 velocityNormalized = rb.velocity;
        velocityNormalized.Normalize();

        transform.LookAt(mouseTarget.transform);

        /*
        if (velocityNormalized != transform.forward)
        {
            //Point plane velocity in the transform.forward direction
        }
        
        rb.AddTorque(Vector3.forward * deltaTime * mouseMove.x * rollSpeed);
        rb.AddTorque(-Vector3.forward * deltaTime * rollDrag * rb.angularVelocity.z);
        
        
        rb.AddTorque(Vector3.right * deltaTime * mouseMove.y * pitchSpeed);
        rb.AddTorque(-Vector3.right * deltaTime * pitchDrag * rb.angularVelocity.x);
        
        
        //rb.AddTorque(Vector3.forward * Time.deltaTime * (mouseMove.x * yawSpeed - yawDrag * rb.angularVelocity.y));
        rb.AddTorque(Vector3.up * deltaTime * horizontalInput * yawSpeed);
        rb.AddTorque(-Vector3.up * deltaTime * yawDrag * rb.angularVelocity.y);
        
        
        if(transform.rotation != Quaternion.identity && mouseMove == Vector2.zero && horizontalInput == 0)
        {
            //transform.rotation = Quaternion.identity;
        }
        */
        //Transform method
        //transform.Translate(Vector3.forward * thrust * Time.deltaTime * verticalInput);
        //transform.Rotate(Vector3.forward * Time.deltaTime * mouseX * rollSpeed);
        //transform.Rotate(Vector3.right * Time.deltaTime * mouseY * pitchSpeed);
        //transform.Rotate(Vector3.up * Time.deltaTime * horizontalInput * yawSpeed);
    }


}
