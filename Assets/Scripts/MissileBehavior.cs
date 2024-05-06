using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    public float missileSpeed; // Missile speed
    public int boatDamage; // Damage to boats
    public int planeDamage; // Damage to planes
    public float homingRadius; // Radius within which the missile homes in
    public float homingStrength; // Strength of homing effect
    private Rigidbody rb;
    private Transform target; // Current target
    private bool isHoming = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * missileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHoming)
        {
            CheckForTargets();
        }
        else
        {
            Homing();
        }
    }

    private void CheckForTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, homingRadius);

        foreach (Collider collider in colliders)
        {
            // if (CheckTag.IsBoat(collider) || CheckTag.IsPlane(collider))
            if (collider.CompareTag("blue-boat") || collider.CompareTag("red-boat") || collider.CompareTag("blue-plane") || collider.CompareTag("red-plane"))
            {
                // Check if the target is of matching color
                if (!CheckTag.MatchingColor(gameObject.tag, collider.tag))
                {
                    isHoming = true;
                    target = collider.transform;
                    break;
                }
            }
        }
    }

    private void Homing()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 homingForce = direction * homingStrength * Time.deltaTime;
            rb.velocity = homingForce.normalized * missileSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is a boat or a plane
        if (CheckTag.IsBoat(other) && !CheckTag.MatchingColor(gameObject.tag, other.tag))
        {
            {
                other.GetComponent<Health>().TakeDamage(boatDamage);
                Destroy(gameObject);
            }
        }
        else if (CheckTag.IsPlane(other) && !CheckTag.MatchingColor(gameObject.tag, other.tag))
        {
                other.GetComponent<Health>().TakeDamage(planeDamage);
                Destroy(gameObject);
        }
    }
}
