using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPrefab : SpawnListener
{
    void Start(){
        // method called when event triggered
        Response = new UnityEvent<GameObject, object>();
        Response.AddListener(ToCall);
    }

    public void ToCall(GameObject caller, object data){
        // cast plain object into SpawnData
        SpawnData d = data as SpawnData;

        // if the data exists
        if (d != null)
        {
            // spawn the game object into world space
            // save reference to this spawned object
            d.Reference = Instantiate(d.Prefab, d.Position, d.Rotation);
        }
    }
}
