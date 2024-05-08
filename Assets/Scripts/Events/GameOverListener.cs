using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameOverEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<GameObject, GameOverData> Response;

    private void OnEnable()
    {
        // register with the spawn event
        Event = FindFirstObjectByType<GameOverEvent>() as GameOverEvent;

        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject caller, GameOverData data){
        Response.Invoke(caller, data);
    }
}
