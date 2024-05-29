using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : Projectile
{
    public GameObject explosionEffect;
    private float bombSpeed = 150f;
    private GameLogic gCtrl;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bombSpeed;
        Invoke("Explode", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        // Check if the other object is a boat or a plane
        if (CheckTag.IsBoat(o) && !CheckTag.MatchingColor(this.gameObject.tag, o.tag))
        {
            Explode();
        }
        else if (CheckTag.IsPlane(o) && !CheckTag.MatchingColor(this.gameObject.tag, o.tag))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            ExplosionBehavior explosionDamage = explosion.GetComponent<ExplosionBehavior>();
            if (explosionDamage != null)
            {
                explosionDamage.Initialize(this.gameObject.tag);
            }
            // Destroy(explosion, 2f);
        }
    }
}
