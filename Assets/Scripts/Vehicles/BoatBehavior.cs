using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BoatBehavior : Vehicle
{
    [Tooltip("Recommend settings\nAcceleration: [0-10]\nMaxThrust: [0-10]")]
    private float maxForwardSpeed = 20f;
    private float rotationalSpeed = 35f;
    private Vector3 rotationVector;
    public float currentThrust;

    private float thrustChangeRate = 20f;
    private float accelerationTime = 5f;
    private float decelerationTime = 5f;
    private float rotationAccelerationTime = 1f;
    private float rotationDecelerationTime = 1f;
    private float currentRotationalThrust = 0f;
    private float rotationalThrustChangeRate = 20f;

    public InputAction forward;
    public InputAction backward;
    public InputAction right;
    public InputAction left;

    private Rigidbody rb;

    private GameLogic gCtrl;

    void Start()
    {
        gCtrl = GameLogic.Instance;
        forward.Enable();
        backward.Enable();
        left.Enable();
        right.Enable();

        rb = this.GetComponent<Rigidbody>();
        rb.SetDensity(10);
        thrustChangeRate = 100f / accelerationTime;
        rotationalThrustChangeRate = rotationalSpeed / rotationAccelerationTime;
    }

    private void MovePlayer()
    {
        float time = Time.deltaTime;

        Vector3 newPos = this.transform.position;
        float forwardSpeed = (currentThrust / 100) * maxForwardSpeed;

        Quaternion currentRot = this.transform.rotation;
        newPos += this.transform.forward * time * forwardSpeed;

        currentRotationalThrust = Mathf.Clamp(
            currentRotationalThrust,
            -rotationalSpeed,
            rotationalSpeed
        );
        rotationVector = new Vector2(0, currentRotationalThrust * time);
        rb.MoveRotation(currentRot * Quaternion.Euler(rotationVector));
        rb.MovePosition(newPos);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            gCtrl.HpSystem.ApplyDamage(this.gameObject, 500);
    }

    private void AdjustThrust()
    {
        float time = Time.deltaTime;

        if (forward.IsPressed())
        {
            currentThrust += thrustChangeRate * time;
        }
        else if (backward.IsPressed())
        {
            currentThrust -= thrustChangeRate * time;
        }
        else
        {
            currentThrust = Mathf.Lerp(currentThrust, 0, time / decelerationTime);
        }

        if (left.IsPressed())
        {
            currentRotationalThrust -= rotationalThrustChangeRate * time;
        }
        else if (right.IsPressed())
        {
            currentRotationalThrust += rotationalThrustChangeRate * time;
        }
        else
        {
            currentRotationalThrust = Mathf.Lerp(
                currentRotationalThrust,
                0,
                time / rotationDecelerationTime
            );
        }

        currentThrust = Mathf.Clamp(currentThrust, 0, 100);
    }

    void FixedUpdate()
    {
        MovePlayer();
        AdjustThrust();
    }
}
