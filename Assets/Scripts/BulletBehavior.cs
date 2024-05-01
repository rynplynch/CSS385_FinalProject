using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletSpeed; // Bullet speed
    public int boatDamage; // Damage to boats
    public int planeDamage; // Damage to planes
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            other.GetComponent<Health>().TakeDamage(boatDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Plane"))
        {
            other.GetComponent<Health>().TakeDamage(planeDamage);
            Destroy(gameObject);
        }
    }

}
