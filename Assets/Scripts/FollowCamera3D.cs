using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera3D : MonoBehaviour
{
    public float cameraDistance;
    public float cameraHeight;
    public InputAction toggleCameraMode;
    public float pitchSensitivity = 0.1f;
    public float yawSensitivity = 0.2f;
    public static FollowCamera3D Instance { get; private set; }
    public bool isFollowingMouse = false;
    
    private float defaultCameraDistance;
    private float defaultCameraHeight;
    private GameObject player;
    private bool togglePressed = false;
    private float maxPitchAngle = 20f;
    private float minPitchAngle = -60f;
    private float currentPitch = 0f;
    private float currentYaw = 0f;

    void LateUpdate()
    {
        // toggle camera once when pressed
        if (toggleCameraMode.IsPressed() && !togglePressed)
        {
            HandleCameraToggle();
            togglePressed = true;
        } 
        else if (!toggleCameraMode.IsPressed() && togglePressed)
        {
            togglePressed = false;
        }

        // Follow mouse for boat or default for either
        if (player != null)
        {
            if (player.CompareTag("Boat") && isFollowingMouse)
            {
                SetMouseFollowCamera();
            }
            else
            {
                SetDefaultCamera();
            }
            // set camera position equal to players forward position
            transform.position = player.transform.position - player.transform.forward * cameraDistance;

            // change camera height in relation to player
            transform.position += new Vector3(0, cameraHeight, 0);

            transform.LookAt(player.transform);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            defaultCameraDistance = cameraDistance;
            defaultCameraHeight = cameraHeight;
            toggleCameraMode.Enable();
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Fire mode, camera follows mouse movement
    private void SetMouseFollowCamera()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        currentYaw += mouseDelta.x * yawSensitivity;
        currentPitch -= mouseDelta.y * pitchSensitivity;
        currentPitch = Mathf.Clamp(currentPitch, minPitchAngle, maxPitchAngle);
        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        transform.position = player.transform.position - player.transform.forward + Vector3.up * 5;
    }

    // Default follow camera for boat and plane
    private void SetDefaultCamera()
    {
        cameraDistance = defaultCameraDistance;
        cameraHeight = defaultCameraHeight;
        transform.position = player.transform.position - player.transform.forward * cameraDistance + Vector3.up * cameraHeight;
        transform.LookAt(player.transform);
    }

    // Toggle between boat fire mode camera and follow camera
    private void HandleCameraToggle()
    {
        if (player && player.CompareTag("Boat"))
        {
            isFollowingMouse = !isFollowingMouse;
        }
    }

    public void setFollowedPlayer(GameObject p) => player = p;
    public GameObject getFollowedPlayer() => player;
}
