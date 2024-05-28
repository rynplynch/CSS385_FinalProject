using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneActions : MonoBehaviour
{
    // how we reference game logic
    private GameLogic gCtrl;

    // rigid body that we apply forces to
    Rigidbody rb;

    // variables related to thrust
    // impacts how quickly plane accelerates
    public float thrustScalar = 0.5f;
    private float thrustAcceleration;

    // if true then we apply thrust
    private bool applyThrust = false;

    // variables related to rotation
    // impacts how quickly the plane rotates
    // defined with roll, yaw and pitch
    public float rollScalar = 0.5f;
    private float rollAcceleration = 0;
    public float yawScalar = 0.5f;
    private float yawAcceleration = 0;
    public float pitchScalar = 0.5f;
    private float pitchAcceleration = 0;

    // if true then apply that rotation
    private bool applyRoll = false;
    private bool applyYaw = false;
    private bool applyPitch = false;

    public float forwardUpSplit = 0.5f;
    public float _maxLinearVelocity = 1000f;
    public float _maxAngularVelocity = 20f;

    void Start()
    {
        // grab reference
        gCtrl = GameLogic.Instance;

        // get the rigid body of this plane
        rb = this.transform.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb.maxLinearVelocity = _maxLinearVelocity;
        rb.maxAngularVelocity = _maxLinearVelocity;
    }

    void Update()
    {
        // if the player is performing thrust action
        if (applyThrust)
        {
            float forward = 1 - forwardUpSplit;
            float upward = 1 - forward;
            // add new thrust
            rb.AddRelativeForce(Vector3.forward * (thrustAcceleration * forward) * Time.deltaTime);
            rb.AddRelativeForce(Vector3.up * (thrustAcceleration * upward) * Time.deltaTime);
        }

        // if the player is performing rotation action
        if (applyRoll)
        {
            // solve for roll speed with delta time
            float rollSpeed = rollAcceleration * Time.deltaTime;

            // translate Euler angle into quaternion
            // z: +counter-clockwise, -clockwise
            Quaternion rollRotation = Quaternion.Euler(0, 0, rollSpeed);

            // added rotation to transform
            transform.rotation *= rollRotation;
        }

        // if the player is performing pitch action
        if (applyPitch)
        {
            // solve for pitch speed with delta time
            float pitchSpeed = pitchAcceleration * Time.deltaTime;

            // translate Euler angle into quaternion
            // x: +up, -down
            Quaternion pitchRotation = Quaternion.Euler(pitchSpeed, 0, 0);

            // added rotation to transform
            transform.rotation *= pitchRotation;
        }

        // if the player is performing pitch action
        if (applyYaw)
        {
            // solve for pitch speed with delta time
            float yawSpeed = yawAcceleration * Time.deltaTime;

            // translate Euler angle into quaternion
            // y: +left, -right
            Quaternion yawRotation = Quaternion.Euler(0, yawSpeed, 0);

            // added rotation to transform
            transform.rotation *= yawRotation;
        }
    }

    // when the player create a move event
    public void OnAlterRoll(InputAction.CallbackContext ctx)
    {
        // if the action started
        if (ctx.started)
            // allows thrust to be applied in update method
            applyRoll = true;
        // if action is over
        else if (ctx.canceled)
            applyRoll = false;

        // grab user input
        float input = ctx.ReadValue<float>();

        if (input > 1)
            input = 1;
        else if (input < -1)
            input = -1;

        // gives direction to scalar
        rollAcceleration = input * rollScalar;
    }

    public void OnAlterPitch(InputAction.CallbackContext ctx)
    {
        // if the action started
        if (ctx.started)
            // allows thrust to be applied in update method
            applyPitch = true;
        // if action is over
        else if (ctx.canceled)
            applyPitch = false;

        // read in normalized input
        float input = ctx.ReadValue<float>();
        if (input > 1)
            input = 1;
        else if (input < -1)
            input = -1;

        // gives direction to scalar
        pitchAcceleration = input * pitchScalar;
    }

    public void OnAlterYaw(InputAction.CallbackContext ctx)
    {
        // if the action started
        if (ctx.started)
            // allows thrust to be applied in update method
            applyYaw = true;
        // if action is over
        else if (ctx.canceled)
            applyYaw = false;

        // read in normalized input
        float input = ctx.ReadValue<float>();

        if (input > 1)
            input = 1;
        else if (input < -1)
            input = -1;

        // scale user input
        input *= yawScalar;

        // gives direction to scalar
        yawAcceleration = input * yawScalar;
    }

    public void OnAlterThrust(InputAction.CallbackContext ctx)
    {
        // if the action started
        if (ctx.started)
            // allows thrust to be applied in update method
            applyThrust = true;
        // if action is over
        else if (ctx.canceled)
            applyThrust = false;

        // this is the new force to apply
        thrustAcceleration = ctx.ReadValue<float>() * thrustScalar;
    }

    // when the player create an bullet upgrade event
    public void OnUpgradeBullet(InputAction.CallbackContext ctx)
    {
        // when the action is first triggered
        if (ctx.performed)
        {
            // get the vehicle component
            Vehicle v = this.GetComponent<Vehicle>();

            // which player spawned this vehicle?
            Player p = v.SpawnedBy.GetComponent<Player>();

            // only perform the upgrade if we know who spawned it
            if (p)
                // tell the upgrade system player wants to upgrade
                gCtrl.UpSystem.UpgradeBullet(p);
        }
    }

    // when the player create an bullet upgrade event
    public void OnMissileUpgrade(InputAction.CallbackContext ctx)
    {
        // when the action is first triggered
        if (ctx.performed)
        {
            // get the vehicle component
            Vehicle v = this.GetComponent<Vehicle>();

            // which player spawned this vehicle?
            Player p = v.SpawnedBy.GetComponent<Player>();

            // only perform the upgrade if we know who spawned it
            if (p)
                // tell the upgrade system player wants to upgrade
                gCtrl.UpSystem.UpgradeMissile(p);
        }
    }

    // when the player create a health upgrade event
    public void OnUpgradeHealth(InputAction.CallbackContext ctx)
    {
        // when the action is first triggered
        if (ctx.performed)
        {
            // get the vehicle component
            Vehicle v = this.GetComponent<Vehicle>();

            // which player spawned this vehicle?
            Player p = v.SpawnedBy.GetComponent<Player>();

            // only perform the upgrade if we know who spawned it
            if (p)
                // tell the upgrade system player wants to upgrade
                gCtrl.UpSystem.UpgradeHealth(p);
        }
    }
}
