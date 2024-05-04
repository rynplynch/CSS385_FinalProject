using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DamageEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<DamageListener> eventListeners =
        new List<DamageListener>();

    public void Raise(GameObject caller, DamageData data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(caller, data);
    }

    public void RegisterListener(DamageListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(DamageListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}

// variables and properties for damaging objects
public class DamageData
{
    // constructors
    public DamageData(){}

    public DamageData(GameObject toDamage, int damageToApply, string tagOfProjectile)
    {
        this.ObjectToDamage = toDamage;
        this.DamageToApply = damageToApply;
        this.Tag = tagOfProjectile;
    }

    // used to save reference to Instantiated GameObject
    // will always start out as null, set by SpawnPrefab class
    private GameObject objectToDamage = null;
    public GameObject ObjectToDamage { get => objectToDamage; set => objectToDamage = value; }

    // damage to apply
    private int damageToApply = 0;
    public int DamageToApply { get => damageToApply; set => damageToApply = value; }

    // tag of the projectile
    // important for checking if you're hurting your own team
    private string tag;
    public string Tag { get => tag; set => tag = value; }
}
