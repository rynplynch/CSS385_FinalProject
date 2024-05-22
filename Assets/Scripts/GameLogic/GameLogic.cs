using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    // listens for damage events, tracks health of object
    private HealthSystem hpSystem;
    public HealthSystem HpSystem
    {
        get => hpSystem;
        private set => hpSystem = value;
    }

    // listens for spawn events, tracks team tickets
    private TicketSystem ticker;
    public TicketSystem Ticker
    {
        get => ticker;
        private set => ticker = value;
    }

    // listens for upgrade Input Actions, tracks upgrades
    private UpgradeSystem upSystem;
    public UpgradeSystem UpSystem
    {
        get => upSystem;
        private set => upSystem = value;
    }

    private EndGame gameEnder;
    public GameClock gameClock;
    public BotSystem botSys;

    // data for spawning objects
    private SpawnData player = new SpawnData();
    public SpawnData Player
    {
        get => player;
        private set => player = value;
    }
    private SpawnData sea = new SpawnData();

    // spawn points for vehicles
    public SpawnData BlueBoatSpawn { get; private set; }
    public SpawnData BluePlaneSpawn { get; private set; }
    public SpawnData RedBoatSpawn { get; private set; }
    public SpawnData RedPlaneSpawn { get; private set; }

    public SpawnData mainCamera = new SpawnData();

    // Unity Events
    public UnityEvent updateHpUI;
    public UnityEvent spawnBot;

    // used to trigger events
    public SpawnEvent spawnEvent;
    public LoadEvent loadEvent;
    public DestroyEvent destroyEvent;
    public DamageEvent damageEvent;
    public GameOverEvent gameOverEvent;

    // Start is called before the first frame update
    void Start()
    {
        // instantiates event handlers
        spawnEvent = ScriptableObject.CreateInstance("SpawnEvent") as SpawnEvent;
        loadEvent = ScriptableObject.CreateInstance("LoadEvent") as LoadEvent;
        destroyEvent = ScriptableObject.CreateInstance("DestroyEvent") as DestroyEvent;
        damageEvent = ScriptableObject.CreateInstance("DamageEvent") as DamageEvent;
        gameOverEvent = ScriptableObject.CreateInstance("GameOverEvent") as GameOverEvent;
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
        // load the main menu
        GoToMainMenuAsync();

        // return cursor control
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

        // show the spawn menu when the game starts
        ShowSpawnMenuAsync();
    }

    // attach GameLogic components for running a game
    private IEnumerator DetachComponents()
    {
        // components that react when an event is triggered
        Destroy(loader);
        Destroy(spawner);
        Destroy(destroyer);
        Destroy(HpSystem);
        Destroy(Ticker);
        Destroy(gameEnder);

        // components that are not event listeners
        Destroy(gameClock);
        Destroy(botSys);

        yield return null;
    }

    // attach GameLogic components for running a game
    private IEnumerator AttachComponents()
    {
        // components that react when an event is triggered
        loader = this.gameObject.AddComponent<LoadResources>();
        spawner = this.gameObject.AddComponent<SpawnPrefab>();
        destroyer = this.gameObject.AddComponent<DestroyObject>();
        HpSystem = this.gameObject.AddComponent<HealthSystem>();
        Ticker = this.gameObject.AddComponent<TicketSystem>();
        UpSystem = this.gameObject.AddComponent<UpgradeSystem>();
        gameEnder = this.gameObject.AddComponent<EndGame>();

        // components that are not event listeners
        gameClock = this.gameObject.AddComponent<GameClock>();
        botSys = this.gameObject.AddComponent<BotSystem>();

        yield return null;
    }

    // load in prefabs and set values
    private IEnumerator LoadPrefabs()
    {
        // initialize spawn data
        RedBoatSpawn = new SpawnData();
        RedPlaneSpawn = new SpawnData();
        BlueBoatSpawn = new SpawnData();
        BluePlaneSpawn = new SpawnData();

        // this is how we find our prefab at load time
        Player.Tag = "Player";
        sea.Tag = "sea";
        mainCamera.Tag = "MainCamera";
        RedBoatSpawn.Tag = "red-bt-spawn";
        RedPlaneSpawn.Tag = "red-pln-spawn";
        BlueBoatSpawn.Tag = "blue-bt-spawn";
        BluePlaneSpawn.Tag = "blue-pln-spawn";

        // raise a load even for each prefab
        loadEvent.Raise(this.gameObject, Player);
        loadEvent.Raise(this.gameObject, sea);
        loadEvent.Raise(this.gameObject, mainCamera);
        loadEvent.Raise(this.gameObject, RedBoatSpawn);
        loadEvent.Raise(this.gameObject, RedPlaneSpawn);
        loadEvent.Raise(this.gameObject, BlueBoatSpawn);
        loadEvent.Raise(this.gameObject, BluePlaneSpawn);

        yield return null;
    }

    // initialize starting game objects
    private IEnumerator InitialSpawn()
    {
        spawnEvent.Raise(this.gameObject, mainCamera);
        spawnEvent.Raise(this.gameObject, Player);

        spawnEvent.Raise(this.gameObject, RedBoatSpawn);
        spawnEvent.Raise(this.gameObject, RedPlaneSpawn);
        spawnEvent.Raise(this.gameObject, BlueBoatSpawn);
        spawnEvent.Raise(this.gameObject, BluePlaneSpawn);
        yield return null;
    }

    // delete game objects in game world
    private IEnumerator Despawn()
    {
        destroyEvent.Raise(this.gameObject, new DestoryData(mainCamera.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(Player.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(RedBoatSpawn.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(RedPlaneSpawn.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(BlueBoatSpawn.Reference, 0f));
        destroyEvent.Raise(this.gameObject, new DestoryData(BluePlaneSpawn.Reference, 0f));
        yield return null;
    }

    // load and display the player menu canvas using the scene
    public async void ShowSpawnMenuAsync()
    {
        // load the player menu scene, wait for it to finish the load
        await SceneManager.LoadSceneAsync("SpawnMenu", LoadSceneMode.Additive);
    }

    public async void GoToMainMenuAsync()
    {
        // load the player menu scene, wait for it to finish the load
        await SceneManager.LoadSceneAsync("MainMenu");
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
