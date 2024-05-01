using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this class should be the only object in our hierarchy
public class GameLogic : MonoBehaviour
{
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

    // data for prefabs
    SpawnData redBoat;
    SpawnData sea;

    // actions used to handle inputs
    public InputAction spawn;
    public InputAction load;

    // used to trigger events
    SpawnEvent spawnEvent;
    LoadEvent loadEvent;

    // Start is called before the first frame update
    void Start()
    {
        // will hold all the data for spawning a red boat
        redBoat = new SpawnData();
        sea = new SpawnData();

        // instantiates event handlers
        spawnEvent = ScriptableObject.CreateInstance("SpawnEvent") as SpawnEvent;
        loadEvent = ScriptableObject.CreateInstance("LoadEvent") as LoadEvent;

        // components that react when an event is triggered
        loader = this.gameObject.AddComponent<LoadResources>();
        spawner = this.gameObject.AddComponent<SpawnPrefab>();

        spawn.Enable();
        load.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStates["GameStart"])
            StartCoroutine(GameStart());
        else if (gameStates["GameRunning"])
            Debug.Log("Running");
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
        redBoat.Tag = "red-boat";
        sea.Tag = "sea";

        loadEvent.Raise(this.gameObject, redBoat);
        loadEvent.Raise(this.gameObject, sea);

        yield return null;
    }

    // initialize starting game objects
    private IEnumerator InitialSpawn()
    {
        spawnEvent.Raise(this.gameObject, sea);
        spawnEvent.Raise(this.gameObject, redBoat);
        yield return null;
    }
}

// variables and properties for spawning red boats
public class SpawnData
{
    private string tag;
    public string Tag { get => tag; set => tag = value; }
    private GameObject prefab;
    public GameObject Prefab { get => prefab; set => prefab = value; }
    private Vector3 position;
    public Vector3 Position { get => position; set => position = value; }
    private Quaternion rotation;
    public Quaternion Rotation { get => rotation; set => rotation = value; }
}
