using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public DamageEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<GameObject, GameObject> Response;

    private void OnEnable()
    {
        // register with the spawn event
        Event = FindFirstObjectByType<DamageEvent>() as DamageEvent;

        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject caller, GameObject data){
        Response.Invoke(caller, data);
    }
}
