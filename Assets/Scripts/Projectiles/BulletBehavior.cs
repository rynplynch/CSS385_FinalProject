
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : Projectile
{
    public float bulletSpeed; // Bullet speed
    public int boatDamage; // Damage to boats
    public int planeDamage; // Damage to planes

    private GameLogic gCtrl;
    private Rigidbody rb;


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

    }

    private void OnTriggerEnter(Collider other)
    {
        // game object of whatever we just collided with
        GameObject c = other.gameObject;

        // if the collider created this bullet
        if (IsWhatSpawnedBullet(c))
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
        if (parent &&
            Object.ReferenceEquals(firedBy, parent.gameObject))
            // this bullet was fired by the parent
            return true;
        // if no parent check if collider object fired the bullet
        else if (Object.ReferenceEquals(firedBy, other.gameObject))
            // this bullet was fired by collider
            return true;

        return false;
    }

    private void ApplyDamage(GameObject other){
        // to be passed into damage event
        // first item is object to damage
        // tag is the take of this bullet
        DamageData d = new DamageData();
        d.ObjectToDamage = other.gameObject;
        d.Tag = this.gameObject.tag;

        // if the target is a boat
        if (IsABoat(other))
            // apply boat damage
            d.DamageToApply = boatDamage;
        // if the target is a plane
        else if (IsAPlane(other))
            // apply boat damage
            d.DamageToApply = planeDamage;

        Debug.Log(d.DamageToApply);
        // create damage event
        gCtrl.damageEvent.Raise(this.gameObject, d);

        // destroy this bullet
        gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
    }

    // did we hit a boat?!?!!
    private bool IsABoat(GameObject o)
    {
        return (o.CompareTag("red-boat") ||
                o.CompareTag("blue-boat") ||
                o.CompareTag("blue-boat-bot") ||
                o.CompareTag("red-boat-tag"));
    }

    // did we hit a plane?!?!??!
    private bool IsAPlane(GameObject o)
    {
        return (o.CompareTag("red-plane") ||
                o.CompareTag("blue-plane") ||
                o.CompareTag("blue-plane-bot") ||
                o.CompareTag("red-plane-tag"));
    }
}
