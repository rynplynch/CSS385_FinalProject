using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DestroyEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<DestroyListener> eventListeners =
        new List<DestroyListener>();

    public void Raise(GameObject caller, DestoryData data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(caller, data);
    }

    public void RegisterListener(DestroyListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(DestroyListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}

// variables and properties for spawning objects
public class DestoryData
{
    // used to save reference to Instantiated GameObject
    // will always start out as null, set by SpawnPrefab class
    private GameObject reference = null;
    public GameObject Reference { get => reference; set => reference = value; }
    private float lifeCycle;
    public float LifeCycle { get => lifeCycle; set => lifeCycle = value; }
}
