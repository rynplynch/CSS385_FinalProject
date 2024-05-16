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
        { "GameStart", true },
        { "GameRunning", false },
        { "GameStop", false }
    };

    // componets attached to this object
    // listen for game event and perform an action
    private LoadResources loader;
    private SpawnPrefab spawner;
    private DestroyObject destroyer;
    private ApplyDamage damager;
    private TicketSystem ticker;
    private EndGame gameEnder;
    private GameClock gameClock;
    private UpgradePlayer upgrader;

    // data for spawning objects
    private SpawnData player = new SpawnData();
    private SpawnData sea = new SpawnData();
    private SpawnData blueSpawn = new SpawnData();
    public SpawnData BlueSpawn
    {
        get => blueSpawn;
        private set => blueSpawn = value;
    }
    private SpawnData redSpawn = new SpawnData();
    public SpawnData RedSpawn
    {
        get => redSpawn;
        private set => redSpawn = value;
    }
    public SpawnData mainCamera = new SpawnData();

    // used to trigger events
    public SpawnEvent spawnEvent;
    public LoadEvent loadEvent;
    public DestroyEvent destroyEvent;
    public DamageEvent damageEvent;
    public GameOverEvent gameOverEvent;
    private UpgradeEvent _upgradeEvent;
    public UpgradeEvent UpgradeEvent
    {
        get => _upgradeEvent;
        private set => _upgradeEvent = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        // instantiates event handlers
        spawnEvent = ScriptableObject.CreateInstance("SpawnEvent") as SpawnEvent;
        loadEvent = ScriptableObject.CreateInstance("LoadEvent") as LoadEvent;
        destroyEvent = ScriptableObject.CreateInstance("DestroyEvent") as DestroyEvent;
        damageEvent = ScriptableObject.CreateInstance("DamageEvent") as DamageEvent;
        gameOverEvent = ScriptableObject.CreateInstance("GameOverEvent") as GameOverEvent;
        UpgradeEvent = ScriptableObject.CreateInstance("UpgradeEvent") as UpgradeEvent;
    }

    // Update is called once per frame
    void Update()
    {
        // if the game is stopped
        if (gameStates["GameStop"])
            // don't do anything
            return;
        // if the game is starting
        else if (gameStates["GameStart"])
            // run the start routine
            StartCoroutine(GameStart());
        else if (gameStates["GameRunning"])
            GameLoop();
    }

    public void GameLoop() { }

    // Game Routines

    // tasks performed when a game is over
    public IEnumerator EndGame()
    {
        // change the game state
        gameStates["GameStop"] = true;

        // stop the game clock
        gameClock.IsCounting = false;

        Debug.Log("GAME OVER");
        // yield return Despawn();
        // yield return DetachComponents();

        yield return null;
    }

    // represents tasks that are performed at game start
    private IEnumerator GameStart()
    {
        gameStates["GameStop"] = false;
        gameStates["GameStart"] = false;
        gameStates["GameRunning"] = true;

        yield return AttachComponents();
        yield return LoadPrefabs();
        yield return InitialSpawn();

        // reset the game clock
        gameClock.ResetClock();

        // start the game clock
        gameClock.IsCounting = true;
    }

    // attach GameLogic components for running a game
    private IEnumerator DetachComponents()
    {
        // components that react when an event is triggered
        Destroy(loader);
        Destroy(spawner);
        Destroy(destroyer);
        Destroy(damager);
        Destroy(ticker);
        Destroy(gameEnder);

        // components that are not event listeners
        Destroy(gameClock);

        yield return null;
    }

    // attach GameLogic components for running a game
    private IEnumerator AttachComponents()
    {
        // components that react when an event is triggered
        loader = this.gameObject.AddComponent<LoadResources>();
        spawner = this.gameObject.AddComponent<SpawnPrefab>();
        destroyer = this.gameObject.AddComponent<DestroyObject>();
        damager = this.gameObject.AddComponent<ApplyDamage>();
        ticker = this.gameObject.AddComponent<TicketSystem>();
        gameEnder = this.gameObject.AddComponent<EndGame>();
        upgrader = this.gameObject.AddComponent<UpgradePlayer>();

        // components that are not event listeners
        gameClock = this.gameObject.AddComponent<GameClock>();

        yield return null;
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

    // delete game objects in game world
    private IEnumerator Despawn()
    {
        destroyEvent.Raise(this.gameObject, new DestoryData(mainCamera.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(player.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(blueSpawn.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(redSpawn.Reference, 0f));
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
