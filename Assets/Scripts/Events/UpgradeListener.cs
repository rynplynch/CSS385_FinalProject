using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public UpgradeEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<GameObject, UpgradeData> Response;

    private void OnEnable()
    {
        // register with the spawn event
        Event = FindFirstObjectByType<UpgradeEvent>() as UpgradeEvent;

        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject caller, UpgradeData data)
    {
        Response.Invoke(caller, data);
    }
}
