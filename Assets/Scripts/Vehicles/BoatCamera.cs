using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BoatCamera : MonoBehaviour
{
    private GameLogic gCtrl;

    public float cameraDistance;
    public float cameraHeight;
    public InputAction toggleCameraMode;
    public float pitchSensitivity = 0.1f;
    public float yawSensitivity = 0.2f;
    public bool isFollowingMouse = false;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public GameObject bulletPrefab;

    public InputAction fire;

    private float defaultCameraDistance;
    private float defaultCameraHeight;
    public GameObject player;
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
            if (
                (player.CompareTag("red-boat") | player.CompareTag("blue-boat")) && isFollowingMouse
            )
            {
                SetMouseFollowCamera();
            }
            else
            {
                SetDefaultCamera();
            }
        }

        // fire on cooldown if left mb is pressed and the boat is in firing mode
        if (fire.WasPerformedThisFrame() && Time.time >= nextFireTime)
        {
            if (isFollowingMouse)
            {
                FireFromCenter();
            }
            else
            {
                FireFromSides();
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gCtrl = GameLogic.Instance;
        fire.Enable();
        toggleCameraMode.Enable();
    }

    // Fire bullets from the center when following the mouse
    private void FireFromCenter()
    {
        SpawnData bull = new SpawnData();
        bull.Prefab = bulletPrefab;
        bull.Position = this.gameObject.transform.position;
        bull.Rotation = this.transform.rotation;
        gCtrl.spawnEvent.Raise(this.gameObject, bull);

        if (player.CompareTag("red-boat"))
            bull.Reference.tag = "red-projectile";
        else if (player.CompareTag("blue-boat"))
            bull.Reference.tag = "blue-projectile";
    }

    // Fire bullets from both sides when not following the mouse
    private void FireFromSides()
    {
        float offsetDistance = 5f; // Distance from the center to the side
        Vector3 leftFirePosition =
            player.transform.position - player.transform.right * offsetDistance;
        Vector3 rightFirePosition =
            player.transform.position + player.transform.right * offsetDistance;
        Quaternion leftFireDirection = Quaternion.LookRotation(-player.transform.right);
        Quaternion rightFireDirection = Quaternion.LookRotation(player.transform.right);

        // Fire from the left side
        FireBullet(leftFirePosition, leftFireDirection);

        // Fire from the right side
        FireBullet(rightFirePosition, rightFireDirection);
    }

    // Function to fire a bullet from a given position
    private void FireBullet(Vector3 position, Quaternion direction)
    {
        SpawnData bull = new SpawnData();
        bull.Prefab = bulletPrefab;
        bull.Position = position;
        bull.Rotation = direction;
        gCtrl.spawnEvent.Raise(this.gameObject, bull);

        if (player.CompareTag("red-boat"))
            bull.Reference.tag = "red-projectile";
        else if (player.CompareTag("blue-boat"))
            bull.Reference.tag = "blue-projectile";
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
        transform.position =
            player.transform.position
            - player.transform.forward * cameraDistance
            + Vector3.up * cameraHeight;
        transform.LookAt(player.transform);
    }

    // Toggle between boat fire mode camera and follow camera
    private void HandleCameraToggle()
    {
        if (player && (player.CompareTag("red-boat") | player.CompareTag("blue-boat")))
        {
            isFollowingMouse = !isFollowingMouse;
        }
    }

    public void setFollowedPlayer(GameObject p) => player = p;

    public GameObject getFollowedPlayer() => player;
}
