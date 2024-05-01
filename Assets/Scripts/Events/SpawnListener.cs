using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public SpawnEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<GameObject, object> Response;

    private void OnEnable()
    {
        // register with the spawn event
        Event = FindFirstObjectByType<SpawnEvent>() as SpawnEvent;

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
