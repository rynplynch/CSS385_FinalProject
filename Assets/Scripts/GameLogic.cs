using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this class should be the only object in our hierarchy
public class GameLogic : MonoBehaviour
{
    // make the GameLogic Singleton referable globally
    public static GameLogic Instance { get; private set; }

    // used to track the different state of the game
    private Dictionary<string, bool> gameStates = new Dictionary<string, bool>
    {
        {"GameStart", true},
        {"GameRunning", false},
        {"GameStop", false}
    };

    // componets attached to this object
    private LoadResources loader;
    private SpawnPrefab spawner;
    private DestroyObject destroyer;

    // data for spawning objects
    private SpawnData player = new SpawnData();
    private SpawnData sea = new SpawnData();
    private SpawnData blueSpawn = new SpawnData();
    public SpawnData BlueSpawn { get => blueSpawn; private set => blueSpawn = value;}
    private SpawnData redSpawn = new SpawnData();
    public SpawnData RedSpawn { get => redSpawn; private set => redSpawn = value;}
    public SpawnData mainCamera = new SpawnData();

    // used to trigger events
    public SpawnEvent spawnEvent;
    public LoadEvent loadEvent;
    public DestroyEvent destroyEvent;

    // Start is called before the first frame update
    void Start()
    {
        // instantiates event handlers
        spawnEvent = ScriptableObject.CreateInstance("SpawnEvent") as SpawnEvent;
        loadEvent = ScriptableObject.CreateInstance("LoadEvent") as LoadEvent;
        destroyEvent = ScriptableObject.CreateInstance("DestroyEvent") as DestroyEvent;

        // components that react when an event is triggered
        loader = this.gameObject.AddComponent<LoadResources>();
        spawner = this.gameObject.AddComponent<SpawnPrefab>();
        destroyer = this.gameObject.AddComponent<DestroyObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameStates["GameStart"])
            StartCoroutine(GameStart());
    }

    // Game Routines

    // represents tasks that are performed at game start
    private IEnumerator GameStart()
    {
        gameStates["GameStart"] = false;
        gameStates["GameRunning"] = true;
        yield return LoadPrefabs();
        yield return InitialSpawn();
    }

    // load in prefabs and set values
    private IEnumerator LoadPrefabs()
    {
        // this is how we find our prefab at load time
        player.Tag = "Player";
        sea.Tag = "sea";
        mainCamera.Tag = "MainCamera";
        redSpawn.Tag = "red-spawn";
        blueSpawn.Tag = "blue-spawn";

        // raise a load even for each prefab
        loadEvent.Raise(this.gameObject, player);
        loadEvent.Raise(this.gameObject, sea);
        loadEvent.Raise(this.gameObject, mainCamera);
        loadEvent.Raise(this.gameObject, blueSpawn);
        loadEvent.Raise(this.gameObject, redSpawn);

        yield return null;
    }

    // initialize starting game objects
    private IEnumerator InitialSpawn()
    {
        spawnEvent.Raise(this.gameObject, mainCamera);
        spawnEvent.Raise(this.gameObject, player);
        spawnEvent.Raise(this.gameObject, blueSpawn);
        spawnEvent.Raise(this.gameObject, redSpawn);
        yield return null;
    }

    // this is here as a safety check
    // it makes sure there is only ever 1 instance of this class
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
           Instance = this;
        }
     }
}

