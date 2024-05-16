using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<UpgradeListener> eventListeners = new List<UpgradeListener>();

    public void Raise(GameObject caller, UpgradeData data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(caller, data);
    }

    public void RegisterListener(UpgradeListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(UpgradeListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}

// variables and properties for damaging objects
public class UpgradeData
{
    // constructors
    public UpgradeData() { }

    private int _playerGold;

    /// <summary>
    /// How much gold the player has
    /// </summary>
    /// <value>int</value>
    public int PlayerGold
    {
        get { return this._playerGold; }
        set { this._playerGold = value; }
    }
}
