using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public DestroyEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<GameObject, object> Response;

    private void OnEnable()
    {
        // register with the spawn event
        Event = FindFirstObjectByType<DestroyEvent>() as DestroyEvent;

        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject caller, object data){
        Response.Invoke(caller, data);
    }
}
