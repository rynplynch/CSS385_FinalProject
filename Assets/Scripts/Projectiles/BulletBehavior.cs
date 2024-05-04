
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
        // check if we are colliding with what fired this bullet
        if (!Object.ReferenceEquals(firedBy, other.gameObject)){
            // to be passed into damage event
            // first item is object to damage
            // second is lifetime
            DamageData d = new DamageData();
            d.ObjectToDamage = other.gameObject;
            d.Tag = this.gameObject.tag;

            if (other.CompareTag("red-boat") | other.CompareTag("blue-boat"))
                d.DamageToApply = boatDamage;
            else if (other.CompareTag("red-plane") | other.CompareTag("blue-plane"))
                d.DamageToApply = planeDamage;

            // create damage event
            gCtrl.damageEvent.Raise(this.gameObject, d);

            // destroy this bullet
            gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
        }
    }

}
