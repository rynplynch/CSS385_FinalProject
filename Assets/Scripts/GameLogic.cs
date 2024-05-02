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

        // raise a load even for each prefab
        loadEvent.Raise(this.gameObject, player);
        loadEvent.Raise(this.gameObject, sea);
        loadEvent.Raise(this.gameObject, mainCamera);

        yield return null;
    }

    // initialize starting game objects
    private IEnumerator InitialSpawn()
    {
        spawnEvent.Raise(this.gameObject, mainCamera);
        spawnEvent.Raise(this.gameObject, player);
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
