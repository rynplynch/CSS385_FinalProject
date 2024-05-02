using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<SpawnListener> eventListeners =
        new List<SpawnListener>();

    public void Raise(GameObject caller, object data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(caller, data);
    }

    public void RegisterListener(SpawnListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(SpawnListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}

// variables and properties for spawning objects
public class SpawnData
{
    // used to save reference to Instantiated GameObject
    // will always start out as null, set by SpawnPrefab class
    private GameObject reference = null;
    public GameObject Reference { get => reference; set => reference = value; }
    private string tag;
    public string Tag { get => tag; set => tag = value; }
    private GameObject prefab;
    public GameObject Prefab { get => prefab; set => prefab = value; }
    private Vector3 position;
    public Vector3 Position { get => position; set => position = value; }
    private Quaternion rotation;
    public Quaternion Rotation { get => rotation; set => rotation = value; }
}
