using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : Projectile
{
    public float bulletSpeed; // Bullet speed
    public int boatDamage; // Damage to boats
    public int planeDamage; // Damage to planes
    public float homingRadius;
    public float homingStrength; // Strength of homing effect

    private GameLogic gCtrl;
    private Rigidbody rb;
    private Transform target; // Current target
    private bool isHoming = false;
    private float targetCheckCoolDown = 0.01f;
    private float lastTargetCheck = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;

        GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // if time since last check is greater than cool down
        // if we are not currently homing
        if (lastTargetCheck > targetCheckCoolDown && !isHoming)
        {
            // grab a new target
            CheckForTargets();
            lastTargetCheck = 0;
        }
        // otherwise we are homing
        else
            Homing();

        lastTargetCheck += Time.deltaTime;
    }

    private void Homing()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 homingForce = direction * homingStrength * Time.deltaTime;
            rb.velocity = homingForce.normalized * bulletSpeed;
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
                    isHoming = true;
                    target = collider.transform;
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // game object of whatever we just collided with
        GameObject c = other.gameObject;

        // if the collider created this bullet or is the world collider
        if (IsWhatSpawnedBullet(c) || IsWorldCollider(c))
            // ignore this collision
            return;

        // now we are sure we are not colliding with what fired us
        ApplyDamage(c);
    }

    // checks if we are colliding with what fired this bullet
    private bool IsWhatSpawnedBullet(GameObject other)
    {
        Transform parent = other.transform.parent;
        // if the parent exists and is what fired this bullet
        if (parent && Object.ReferenceEquals(firedBy, parent.gameObject))
            // this bullet was fired by the parent
            return true;
        // if no parent check if collider object fired the bullet
        else if (Object.ReferenceEquals(firedBy, other.gameObject))
            // this bullet was fired by collider
            return true;

        return false;
    }

    // check if we are colliding with the world collider
    private bool IsWorldCollider(GameObject o)
    {
        WorldBounds wb = o.GetComponent<WorldBounds>();
        // if the component exists we are colliding w/ world bounds
        return wb;
    }

    private void ApplyDamage(GameObject other)
    {
        // to be passed into damage event
        // first item is object to damage
        // tag is the take of this bullet
        DamageData d = new DamageData();
        d.ObjectToDamage = other.gameObject;
        d.Projectile = this.gameObject;

        // if the target is a boat
        if (CheckTag.IsBoat(other))
            // apply boat damage
            d.DamageToApply = boatDamage;
        // if the target is a plane
        else if (CheckTag.IsPlane(other))
            // apply boat damage
            d.DamageToApply = planeDamage;

        // create damage event
        gCtrl.damageEvent.Raise(this.gameObject, d);

        // destroy this bullet
        gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
    }
}
