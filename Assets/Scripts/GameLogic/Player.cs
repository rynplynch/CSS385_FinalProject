using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // player actions
    public InputAction showPlayerMenu;

    // how we reference our game logic and events
    private GameLogic gCtrl;
    private SpawnEvent sEvent;
    private LoadEvent lEvent;
    private DestroyEvent dEvent;
    private UpgradeEvent uEvent;

    // objects the player can spawn as
    SpawnData redBoat = new SpawnData();
    SpawnData redPlane = new SpawnData();
    SpawnData blueBoat = new SpawnData();
    SpawnData bluePlane = new SpawnData();

    // player camera that follows spawned objects
    SpawnData boatCamera = new SpawnData();
    SpawnData planeCamera = new SpawnData();

    // ui canvas
    public GameObject UICanvas;
    public static int playerId;
    private UpgradeManager upgradeManager;

    // singleton for managing player gold
    private GoldManagerScript goldManager;

    void Start()
    {
        // Assign or retrieve player ID when the player is created or initialized
        playerId = GetPlayerId();
        upgradeManager = UpgradeManager.Instance;

        // setting references at creation of player
        gCtrl = GameLogic.Instance;
        sEvent = gCtrl.spawnEvent;
        lEvent = gCtrl.loadEvent;
        dEvent = gCtrl.destroyEvent;
        uEvent = gCtrl.UpgradeEvent;
        goldManager = FindAnyObjectByType<GoldManagerScript>();

        UICanvas = GameObject.FindGameObjectWithTag("ui-canvas");
        // grab reference to prefabs
        LoadPrefabs();

        showPlayerMenu.Enable();
    }

    void Update()
    {
        // Check for input to upgrade
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            // Call the UpgradeMissile method with the player ID
            // upgradeManager.UpgradeHealth(playerId);
            StartCoroutine(UpdateHealthRoutine());
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            // Call the UpgradeBullet method with the player ID
            // upgradeManager.UpgradeBullet(playerId);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            // Call the UpgradeMissile method with the player ID
            // upgradeManager.UpgradeMissile(playerId);
        }

        if (showPlayerMenu.WasPressedThisFrame())
        {
            ShowPlayerMenu();
        }
        else if (showPlayerMenu.WasReleasedThisFrame())
        {
            SceneManager.UnloadSceneAsync("PlayerMenu");
            Scene game = SceneManager.GetSceneByName("Game");
            SceneManager.SetActiveScene(game);
        }
    }

    // load and display the player menu canvas using the scene
    private void ShowPlayerMenu()
    {
        // load the player menu scene
        SceneManager.LoadScene("PlayerMenu", LoadSceneMode.Additive);
    }

    // sequence of events that happens during spawning
    private IEnumerator SpawnRoutine(SpawnData o)
    {
        // returns the vehicle the player is spawned as
        SpawnData currentVehicle = GetSpawnedVehicle();

        // if the player has a spawned vehicle
        if (currentVehicle != null)
        {
            // detach camera from spawned vehicle
            yield return DetachCamera();

            // ready destroy data for destroy event
            DestoryData d = new DestoryData(currentVehicle.Reference, 0);

            // destroy current vehicle
            yield return DeleteObject(d);
        }

        // if the player is spawning as a red team vehicle
        if (o.Tag.Equals("red-boat") | o.Tag.Equals("red-plane"))
            // change the spawn position to red spawn
            // offset a little so it doesn't spawn right on top
            o.Position = gCtrl.RedSpawn.Position + new Vector3(10, -20, 0);

        // if the player is spawning as a red team vehicle
        if (o.Tag.Equals("blue-boat") | o.Tag.Equals("blue-plane"))
            // change the spawn position to red spawn
            // offset a little so it doesn't spawn right on top
            o.Position = gCtrl.BlueSpawn.Position + new Vector3(10, -20, 0);

        yield return SpawnObject(o);
        // this allows health upgrades to persisten between spawns
        yield return ApplyHealthLevel(o);
        yield return AttachCamera(o);
        yield return SetPlayerReference(o);
    }

    // apply the current health level of player vehicle
    private IEnumerator ApplyHealthLevel(SpawnData o)
    {
        // get health component of vehicle
        PlayerHealth hp = o.Reference.GetComponent<PlayerHealth>();

        // get health level of player
        int lvl = gCtrl.upgrader.GetPlayerHealthLvl(this.gameObject);

        // change the max health of the vehicle
        hp.maxHealth = hp.maxHealth + gCtrl.upgrader.healthIncrease * lvl;

        // also update the current health
        hp.currentHealth = hp.maxHealth;

        yield return null;
    }

    private IEnumerator UpdateHealthRoutine()
    {
        // get the players current vehicle
        SpawnData currentVehicle = GetSpawnedVehicle();

        // if the player is spawned allow upgrading
        if (currentVehicle != null)
        {
            // data to pass to the upgraded event
            UpgradeData uData = new UpgradeData();

            // player to upgrade
            uData.Player = this.gameObject;

            // grab the vehicles health comp
            uData.PlayerHealth = currentVehicle.Reference.GetComponent<PlayerHealth>();

            // current amount of gold the player has
            uData.PlayerGold = 50;

            // raise the upgrade event
            uEvent.Raise(this.gameObject, uData);
        }
        yield return null;
    }

    // save reference to this player instance inside spawned game object
    private IEnumerator SetPlayerReference(SpawnData o)
    {
        // grab the vehicle component from the just spawned boat/plane
        Vehicle v = o.Reference.gameObject.GetComponent<Vehicle>();

        // set the spawned by reference
        v.SpawnedBy = this;

        yield return null;
    }

    private IEnumerator AttachCamera(SpawnData o)
    {
        // deactivate main camera
        gCtrl.mainCamera.Reference.SetActive(false);

        // check what the spawn object is
        if (o.Tag.Equals("blue-boat") | o.Tag.Equals("red-boat"))
        {
            // spawn a camera for a boat
            yield return SpawnObject(boatCamera);

            // grab the boat camera script
            BoatCamera script = boatCamera.Reference.GetComponent<BoatCamera>();

            // pass in a refence so the camera can follow the boat
            script.setFollowedPlayer(o.Reference);
        }
        else if (o.Tag.Equals("blue-plane") | o.Tag.Equals("red-plane"))
        {
            yield return SpawnObject(planeCamera);
            PlaneCamera script = planeCamera.Reference.GetComponent<PlaneCamera>();
            script.setFollowedPlayer(o.Reference);
        }

        yield return null;
    }

    private IEnumerator DetachCamera()
    {
        DestoryData d = new DestoryData();

        // the camera that has a reference is the one that is live
        if (boatCamera.Reference)
            d.Reference = boatCamera.Reference;
        else if (planeCamera.Reference)
            d.Reference = planeCamera.Reference;

        d.LifeCycle = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // destroy camera
        yield return DeleteObject(d);
    }

    private IEnumerator SpawnObject(SpawnData o)
    {
        sEvent.Raise(this.gameObject, o);
        yield return null;
    }

    private IEnumerator DeleteObject(DestoryData o)
    {
        if (o.Reference != null)
            dEvent.Raise(this.gameObject, o);
        yield return null;
    }

    // displays red or blue team menu
    private async void ShowTeamMenuAsync(string team)
    {
        // unload the current UI

        // if red is passed in
        if (team.Contains("red"))
            // load up red team menu
            await SceneManager.LoadSceneAsync("RedTeamMenu", LoadSceneMode.Additive);
        // if blue is passed in
        else if (team.Contains("blue"))
            // load up blue team menu
            await SceneManager.LoadSceneAsync("BlueTeamMenu", LoadSceneMode.Additive);
    }

    private void LoadPrefabs()
    {
        // this is how load knows which prefab to grab
        // must match tag assigned to prefab
        redBoat.Tag = "red-boat";
        blueBoat.Tag = "blue-boat";
        bluePlane.Tag = "blue-plane";
        redPlane.Tag = "red-plane";
        boatCamera.Tag = "boat-camera";
        planeCamera.Tag = "plane-camera";

        lEvent.Raise(this.gameObject, redPlane);
        lEvent.Raise(this.gameObject, bluePlane);
        lEvent.Raise(this.gameObject, redBoat);
        lEvent.Raise(this.gameObject, blueBoat);
        lEvent.Raise(this.gameObject, boatCamera);
        lEvent.Raise(this.gameObject, planeCamera);
    }

    // actions taken when the player dies
    // takes in a string that is the team they died as
    public IEnumerator PlayerDied(string t)
    {
        // remove the players camera
        yield return DetachCamera();

        // reactivate main camera
        gCtrl.mainCamera.Reference.SetActive(true);

        // take the player back to their team menu
        ShowTeamMenuAsync(t);
    }

    public void SpawnSelection(string teamVehicle)
    {
        if (teamVehicle == "redBoat")
        {
            StartCoroutine(SpawnRoutine(redBoat));
        }
        if (teamVehicle == "blueBoat")
        {
            StartCoroutine(SpawnRoutine(blueBoat));
        }
        if (teamVehicle == "redPlane")
        {
            StartCoroutine(SpawnRoutine(redPlane));
        }
        if (teamVehicle == "bluePlane")
        {
            StartCoroutine(SpawnRoutine(bluePlane));
        }
    }

    public int GetPlayerId()
    {
        // Retrieve player ID from persistent storage (e.g., PlayerPrefs)
        // If player ID doesn't exist, assign a new unique ID and save it
        int savedPlayerId = PlayerPrefs.GetInt("PlayerId", -1);
        if (savedPlayerId == -1)
        {
            savedPlayerId = Random.Range(0, 10); // Generate a random player ID (or use a different method to ensure uniqueness)
            PlayerPrefs.SetInt("PlayerId", savedPlayerId); // Save player ID to PlayerPrefs
        }
        return savedPlayerId;
    }

    // returns the currently spawned vehicle
    // returns null if no vehicle spawned
    private SpawnData GetSpawnedVehicle()
    {
        if (redBoat.Reference != null)
            return redBoat;
        if (blueBoat.Reference != null)
            return blueBoat;
        if (redPlane.Reference != null)
            return redPlane;
        if (bluePlane.Reference != null)
            return bluePlane;
        return null;
    }
}
