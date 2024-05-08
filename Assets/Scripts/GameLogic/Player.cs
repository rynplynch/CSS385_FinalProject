using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // player camera that follows spawned objects
    SpawnData boatCamera = new SpawnData();
    SpawnData planeCamera = new SpawnData();

    public static int playerId;
    private UpgradeManager upgradeManager;

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

        // grab reference to prefabs
        LoadPrefabs();
    }

    void Update()
    {
        // Check for input to upgrade
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            // Call the UpgradeMissile method with the player ID
            upgradeManager.UpgradeHealth(playerId);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            // Call the UpgradeBullet method with the player ID
            upgradeManager.UpgradeBullet(playerId);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            // Call the UpgradeMissile method with the player ID
            upgradeManager.UpgradeMissile(playerId);
        }      
    }

    // sequence of events that happens during spawning
    private IEnumerator SpawnRoutine(SpawnData o){
        DestoryData d = new DestoryData();
        d.LifeCycle = 0;
        // make sure the player can not spawn as multiple GameObjects at once
        // also detach the player-camera if that is the case
        if (redBoat.Reference != null){
            yield return DetachCamera();
            d.Reference = redBoat.Reference;
            yield return DeleteObject(d);
        }
        if (blueBoat.Reference != null){
            yield return DetachCamera();
            d.Reference = blueBoat.Reference;
            yield return DeleteObject(d);
        }
        if (redPlane.Reference != null){
            yield return DetachCamera();
            d.Reference = redPlane.Reference;
            yield return DeleteObject(d);
        }
        if (bluePlane.Reference != null){
            yield return DetachCamera();
            d.Reference = bluePlane.Reference;
            yield return DeleteObject(d);
        }

        // if the player is spawning as a red team vehicle
        if (o.Tag.Equals("red-boat") | o.Tag.Equals("red-plane"))
            // change the spawn position to red spawn
            // offset a little so it doesn't spawn right on top
            o.Position = gCtrl.RedSpawn.Position + new Vector3(10,-20,0);

        // if the player is spawning as a red team vehicle
        if (o.Tag.Equals("blue-boat") | o.Tag.Equals("blue-plane"))
            // change the spawn position to red spawn
            // offset a little so it doesn't spawn right on top
            o.Position = gCtrl.BlueSpawn.Position + new Vector3(10,-20,0);

        yield return SpawnObject(o);
        yield return AttachCamera(o);
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
        if( o.Reference != null )
            dEvent.Raise(this.gameObject, o);
        yield return null;
    }

    private void LoadPrefabs(){
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
}
