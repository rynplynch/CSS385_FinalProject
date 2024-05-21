using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyObject : DestroyListener
{
    void Start()
    {
        // method called when event triggered
        Response = new UnityEvent<GameObject, DestoryData>();
        Response.AddListener(ToCall);
        GameLogic gCtrl = GameLogic.Instance;
    }

    public void ToCall(GameObject caller, DestoryData d)
    {
        // if the data exists and there is a reference to instantiated object
        if (d != null && d.Reference != null)
        {
            // if there is a vehicle component
            Vehicle v = d.Reference.GetComponent<Vehicle>();

            if (v)
            {
                // the instantiated object tells us who create it
                GameObject o = v.SpawnedBy;

                // grab player component
                Player p = o.GetComponent<Player>();

                // if the player was on the blue team
                if (CheckTag.IsBlueTeam(d.Reference))
                    // let the player know they've died on the blue team
                    StartCoroutine(p.PlayerDied("blue"));
                else if (CheckTag.IsRedTeam(d.Reference))
                    // let the player know they've died on the red team
                    StartCoroutine(p.PlayerDied("red"));
            }
            // destroy the reference
            Destroy(d.Reference, d.LifeCycle);

            // be sure to set reference back to null
            // idk maybe it does this itself?
            d.Reference = null;
        }
    }
}
