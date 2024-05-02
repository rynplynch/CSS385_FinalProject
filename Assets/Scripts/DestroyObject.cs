using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyObject : DestroyListener
{
    void Start(){
        // method called when event triggered
        Response = new UnityEvent<GameObject, object>();
        Response.AddListener(ToCall);
    }

    public void ToCall(GameObject caller, object data){
        // cast plain object into SpawnData
        SpawnData d = data as SpawnData;

        // if the data exists and there is a reference to instantiated object
        if (d != null && d.Reference != null){
            // destroy the reference
            Destroy(d.Reference);

            // be sure to set reference back to null
            d.Reference = null;
        }
    }
}
