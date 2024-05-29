using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : Projectile
{
    private int planeDamage = 200;
    private int boatDamage = 300;
    private GameLogic gCtrl;

    public void Initialize(string tag)
    {
        this.tag = tag;
    }

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
        Invoke("Explode", 1.5f);
    }

    // Update is called once per frame
    void Update() { }

    private void Explode()
    {
        // destroy self
        gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
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
        }
        else if (CheckTag.IsPlane(o) && !CheckTag.MatchingColor(this.gameObject.tag, o.tag))
        {
            // raise a new damage event
            gCtrl.damageEvent.Raise(
                this.gameObject,
                new DamageData(o, planeDamage, this.gameObject)
            );
        }
    }
}
