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
        Response = new UnityEvent<GameObject, GameObject>();

        // tells the event to call this function
        Response.AddListener(ToCall);
    }

    private void ToCall(GameObject caller, object data){
        Debug.Log(data);
    }
}
