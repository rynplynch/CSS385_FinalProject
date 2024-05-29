using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // how we reference our game logic and events
    private GameLogic gCtrl;
    private SpawnEvent sEvent;
    private LoadEvent lEvent;
    private DestroyEvent dEvent;

    // objects the player can spawn as
    SpawnData redBoat = new SpawnData();
    SpawnData redPlane = new SpawnData();
    SpawnData blueBoat = new SpawnData();
    SpawnData bluePlane = new SpawnData();

    // allowed spawn radius
    public int spawnRadius = 25;

    // player camera that follows spawned objects
    SpawnData boatCamera = new SpawnData();
    SpawnData planeCamera = new SpawnData();

    // ui canvas
    public GameObject UICanvas;
    public static int playerId;

    // singleton for managing player gold
    private GoldManagerScript goldManager;

    void Start()
    {
        // Assign or retrieve player ID when the player is created or initialized
        playerId = GetPlayerId();

        // setting references at creation of player
        gCtrl = GameLogic.Instance;
        sEvent = gCtrl.spawnEvent;
        lEvent = gCtrl.loadEvent;
        dEvent = gCtrl.destroyEvent;
        goldManager = FindAnyObjectByType<GoldManagerScript>();

        UICanvas = GameObject.FindGameObjectWithTag("ui-canvas");
        // grab reference to prefabs
        LoadPrefabs();
    }

    void Update() { }

    // sequence of events that happens during spawning
    private IEnumerator SpawnRoutine(SpawnData o)
    {
        // returns the vehicle the player is spawned as
        GameObject currentVehicle = GetSpawnedVehicle();

        // if the player has a spawned vehicle
        if (currentVehicle != null)
        {
            // detach camera from spawned vehicle
            yield return DetachCamera();

            // ready destroy data for destroy event
            DestoryData d = new DestoryData(currentVehicle, 0);

            // destroy current vehicle
            yield return DeleteObject(d);
        }

        // sets the spawn point of the object
        yield return SetSpawnPoint(o);

        // spawn the object
        yield return SpawnObject(o);

        // this persists health upgrades
        gCtrl.UpSystem.UpgradeNewVehicle(this, o.Reference);

        // attach camera so player can follow object
        yield return AttachCamera(o);

        // how we track if a vehicle belongs to the player
        yield return SetPlayerReference(o);

        // display UI for vehicles
        ShowVehicleUI();
    }

    // save reference to this player instance inside spawned game object
    private IEnumerator SetPlayerReference(SpawnData o)
    {
        // grab the vehicle component from the just spawned boat/plane
        Vehicle v = o.Reference.gameObject.GetComponent<Vehicle>();

        if (!v)
            v = o.Reference.gameObject.GetComponentInChildren<Vehicle>();
        // set the spawned by reference
        v.SpawnedBy = this.gameObject;

        yield return null;
    }

    // sets the spawn point inside the spawn data
    private IEnumerator SetSpawnPoint(SpawnData o)
    {
        // height vehicles are spawned
        float height = 2f;

        // construct offset vector
        Vector3 offset = GetRandomVec(spawnRadius);

        // add the offset to vehicle spawn point
        o.Position = offset;

        // if the player is spawning as a red team vehicle
        if (CheckTag.IsRedTeam(o.Prefab))
        {
            // if the player is spawning as a boat
            if (CheckTag.IsBoat(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.RedBoatSpawn.Position + offset;

                // spawn facing toward blue spawn
                o.Rotation = Quaternion.LookRotation(Vector3.right);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
            // if the player is spawning as a plane
            else if (CheckTag.IsPlane(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.RedPlaneSpawn.Position + offset;

                // spawn facing toward blue spawn
                o.Rotation = Quaternion.LookRotation(Vector3.right);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
        }
        // if the player is spawning as a red team vehicle
        else if (CheckTag.IsBlueTeam(o.Prefab))
            // if the player is spawning as a boat
            if (CheckTag.IsBoat(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.BlueBoatSpawn.Position + offset;

                // spawn facing toward red spawn
                o.Rotation = Quaternion.LookRotation(Vector3.left);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
            // if the player is spawning as a plane
            else if (CheckTag.IsPlane(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.BluePlaneSpawn.Position + offset;

                // spawn facing toward red spawn
                o.Rotation = Quaternion.LookRotation(Vector3.left);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
        yield return null;
    }

    private IEnumerator AttachCamera(SpawnData o)
    {
        // deactivate main camera
        gCtrl.mainCamera.Reference.SetActive(false);

        // check what the spawn object is
        if (CheckTag.IsBoat(o.Prefab))
        {
            // spawn a camera for a boat
            yield return SpawnObject(boatCamera);

            // grab the boat camera script
            BoatCamera script = boatCamera.Reference.GetComponent<BoatCamera>();

            // pass in a refence so the camera can follow the boat
            script.setFollowedPlayer(o.Reference);
        }
        else if (CheckTag.IsPlane(o.Prefab))
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

    // decide what vehicle UI to show
    private void ShowVehicleUI()
    {
        // the vehicle the player currently control's
        GameObject o = GetSpawnedVehicle();

        // if its a boat
        if (CheckTag.IsBoat(o))
            // show boat UI
            ShowBoatUIAsync();
        else if (CheckTag.IsPlane(o))
            // show plane UI
            ShowPlaneUIAsync();
    }

    // displays the boat UI
    private async void ShowBoatUIAsync()
    {
        if (!SceneManager.GetSceneByName("BoatUI").isLoaded)
            await SceneManager.LoadSceneAsync("BoatUI", LoadSceneMode.Additive);
    }

    // displays the plane UI
    private async void ShowPlaneUIAsync()
    {
        await SceneManager.LoadSceneAsync("PlaneUI", LoadSceneMode.Additive);
    }

    // displays red or blue team menu
    private async void ShowTeamMenuAsync(string team)
    {
        // if red is passed in
        if (team.Contains("red"))
            // load up red team menu
            await SceneManager.LoadSceneAsync("RedTeamMenu", LoadSceneMode.Additive);
        // if blue is passed in
        else if (team.Contains("blue"))
            // load up blue team menu
            await SceneManager.LoadSceneAsync("BlueTeamMenu", LoadSceneMode.Additive);
    }

    private async void HideVehicleUI()
    {
        // if the vehicle was a boat and if the scene is loaded
        if (CheckTag.IsBoat(GetSpawnedVehicle()) && SceneManager.GetSceneByBuildIndex(7).IsValid())
            // remove boat UI
            await SceneManager.UnloadSceneAsync("BoatUI");
        // if the vehicle was a plane and if the scene is loaded
        else if (
            CheckTag.IsPlane(GetSpawnedVehicle()) && SceneManager.GetSceneByBuildIndex(8).IsValid()
        )
            // remove the plane UI
            await SceneManager.UnloadSceneAsync("PlaneUI");
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
        // remove vehicle UI
        HideVehicleUI();

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
    public GameObject GetSpawnedVehicle()
    {
        if (redBoat.Reference != null)
            return redBoat.Reference;
        if (blueBoat.Reference != null)
            return blueBoat.Reference;
        if (redPlane.Reference != null)
            return redPlane.Reference;
        if (bluePlane.Reference != null)
            return bluePlane.Reference;
        return null;
    }

    // returns a random vector3 with the specified range
    private Vector3 GetRandomVec(int range)
    {
        // vectore to return
        Vector3 randomVec = new Vector3();

        // get random for each component
        randomVec.x = Random.Range(-range, range);
        randomVec.y = Random.Range(-range, range);
        randomVec.z = Random.Range(-range, range);

        return randomVec;
    }
}
