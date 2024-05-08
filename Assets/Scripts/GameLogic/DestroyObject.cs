using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyObject : DestroyListener
{
    void Start(){
        // method called when event triggered
        Response = new UnityEvent<GameObject, DestoryData>();
        Response.AddListener(ToCall);
        GameLogic gCtrl = GameLogic.Instance;
    }

    public void ToCall(GameObject caller, DestoryData data){
        // cast plain object into SpawnData
        DestoryData d = data;

        // if the data exists and there is a reference to instantiated object
        if (d != null && d.Reference != null){
            // destroy the reference
            Destroy(d.Reference, d.LifeCycle);

            // be sure to set reference back to null
            d.Reference = null;
        }
    }
}
