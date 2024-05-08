using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnCameraBehavior : MonoBehaviour
{
    public Transform center;
    public float orbitSpeed = 5.0f;
    public float distanceFromCenter = 100.0f;
    public float heightAboveCenter = 100.0f; 

    private void Start()
    {
        Vector3 offset = new Vector2(distanceFromCenter, heightAboveCenter);
        transform.position = center.position + transform.rotation * offset;
    }
    void Update()
    {
        transform.LookAt(center);

        transform.Translate(Vector3.right * (orbitSpeed * Time.deltaTime));
    }
}
