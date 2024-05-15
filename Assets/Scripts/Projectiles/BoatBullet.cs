using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBullet : Projectile
{
    public float speed = 20f;
    public float lifespan = 3f;
    public int damage;

    private GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;

        // pass in the caller, then the destroy data
        gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, lifespan));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        // check if we are colliding with what fired us
        if (!Object.ReferenceEquals(firedBy, other.gameObject))
        {
            // create damage event
            gCtrl.damageEvent.Raise(this.gameObject, new DamageData(other.gameObject, damage, this.gameObject.tag));

            // destroy self
            gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
        }
    }
}
