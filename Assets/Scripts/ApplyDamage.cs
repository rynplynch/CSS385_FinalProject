using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ApplyDamage : DamageListener
{
    // Start is called before the first frame update
    void Start()
    {
        // instantiate new unity event
        Response = new UnityEvent<GameObject, DamageData>();

        // tells the event to call this function
        Response.AddListener(ToCall);
    }

    private void ToCall(GameObject caller, DamageData d){
        // grab health component of object to damage
        Health hp = d.ObjectToDamage.GetComponent<Health>();

        // only do damage if the object has a health component
        if (hp)
        {
            // if we are on the same team
            if (IsSameTeam(d))
                // break from the function
                return;

            hp.TakeDamage(d.DamageToApply);
        }
    }

    private bool IsSameTeam(DamageData d)
    {
        // get tag of the object to damage
        string theirTag = d.ObjectToDamage.tag;

        // get the tag of the projectile
        string ourTag = d.Tag;

        // are we all on the red team?
        if ((theirTag.Equals("red-boat") || theirTag.Equals("red-plane")) && ourTag.Equals("red-projectile"))
            return true;

        // also include the bots!
        if ((theirTag.Equals("red-boat-bot") || theirTag.Equals("red-plane-bot")) && ourTag.Equals("red-projectile"))
            return true;

        // are we all on the blue team?
        if ((theirTag.Equals("blue-boat") || theirTag.Equals("blue-plane")) && ourTag.Equals("blue-projectile"))
            return true;

        // also include the bots!
        if ((theirTag.Equals("blue-boat-bot") || theirTag.Equals("blue-plane-bot")) && ourTag.Equals("blue-projectile"))
            return true;

        return false;
    }
}
