using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private LoadEvent loadEvent = new LoadEvent();
    private SpawnEvent spawnEvent = new SpawnEvent();

    // load event variables
    GameLogic gameLogic;
    public GameLogic GameLogic{ get => gameLogic; set => gameLogic = value; }

    public LoadEvent GetLoadEvent
    {
        get
        {
            if (loadEvent == null) loadEvent = new LoadEvent();
            return loadEvent;
        }
    }

    public SpawnEvent GetSpawnEvent
    {
        get
        {
            if (spawnEvent == null)  spawnEvent = new SpawnEvent();
            return spawnEvent;
        }
    }

    public void CallLoad(GameLogic gameLogic)
    {
        if(this.gameObject) this.gameLogic = gameLogic;

        loadEvent.CallLoadEvent();
    }

    public void CallSpawn(GameLogic gameLogic)
    {
        if(this.gameObject) this.gameLogic = gameLogic;

        spawnEvent.CallSpawnEvent();
    }
}

public class LoadEvent
{
    public delegate void InteractHandler();

    public event InteractHandler HasInteracted;

    public void CallLoadEvent() => HasInteracted?.Invoke();
}

public class SpawnEvent
{
    public delegate void InteractHandler();

    public event InteractHandler HasInteracted;

    public void CallSpawnEvent() => HasInteracted?.Invoke();
}
