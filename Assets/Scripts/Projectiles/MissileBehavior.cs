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
    private GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
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
            GameObject o = collider.gameObject;
            if (o && (CheckTag.IsBoat(o) || CheckTag.IsPlane(o)))
            {
                // Check if the target is of matching color
                if (!CheckTag.MatchingColor(this.gameObject.tag, collider.tag))
                {
                    Debug.Log(collider.name);
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
        GameObject o = other.gameObject;
        // Check if the other object is a boat or a plane
        if (CheckTag.IsBoat(o) && !CheckTag.MatchingColor(this.gameObject.tag, o.tag))
        {
            // raise a new damage event
            gCtrl.damageEvent.Raise(
                this.gameObject,
                new DamageData(o, boatDamage, this.gameObject)
            );

            // raise a new destroy event
            gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
        }
        else if (CheckTag.IsPlane(o) && !CheckTag.MatchingColor(this.gameObject.tag, o.tag))
        {
            // raise a new damage event
            gCtrl.damageEvent.Raise(
                this.gameObject,
                new DamageData(o, planeDamage, this.gameObject)
            );

            // raise a new destroy event
            gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
        }
    }
}
