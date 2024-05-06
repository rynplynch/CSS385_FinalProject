using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPrefab : SpawnListener
{
    void Start(){
        // method called when event triggered
        Response = new UnityEvent<GameObject, SpawnData>();
        Response.AddListener(ToCall);
    }

    public void ToCall(GameObject caller, SpawnData data){
        // spawn the game object into world space
        // save reference to this spawned object
        data.Reference = Instantiate(data.Prefab, data.Position, data.Rotation);

        Projectile p = data.Reference.GetComponent<Projectile>();
        // if the game object has a projectile component
        if (p)
            p.firedBy = caller;
    }
}
