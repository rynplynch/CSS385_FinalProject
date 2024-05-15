using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ApplyDamage : DamageListener
{
    public int goldValue = 15;
    
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
            if (CheckTag.MatchingColor(d.Tag, d.ObjectToDamage.tag))
                // break from the function
                return;

            hp.TakeDamage(d.DamageToApply);
            FindFirstObjectByType<GoldManagerScript>().AddGold(Player.playerId, goldValue);
        }
    }
}
