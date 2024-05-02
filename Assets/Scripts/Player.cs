using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction spawnAsRedBoat;
    public InputAction spawnAsRedPlane;
    public InputAction spawnAsBlueBoat;
    public InputAction spawnAsBluePlane;

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
    SpawnData playerCamera = new SpawnData();

    void Start()
    {
        // setting references at creation of player
        gCtrl = GameLogic.Instance;
        sEvent = gCtrl.spawnEvent;
        lEvent = gCtrl.loadEvent;
        dEvent = gCtrl.destroyEvent;

        // grab reference to prefabs
        LoadPrefabs();

        spawnAsRedBoat.Enable();
        spawnAsBlueBoat.Enable();
        spawnAsRedPlane.Enable();
        spawnAsBluePlane.Enable();
    }

    void Update(){
        if (spawnAsRedBoat.WasPerformedThisFrame()){
            StartCoroutine(SpawnRoutine(redBoat));
        }
        if (spawnAsBlueBoat.WasPerformedThisFrame()){
            StartCoroutine(SpawnRoutine(blueBoat));
        }
        if (spawnAsRedPlane.WasPerformedThisFrame()){
            StartCoroutine(SpawnRoutine(redPlane));
        }
        if (spawnAsBluePlane.WasPerformedThisFrame()){
            StartCoroutine(SpawnRoutine(bluePlane));
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

        // spawn the player camera prefab
        yield return SpawnObject(playerCamera);

        // grab the attached script
        FollowCamera3D script = playerCamera.Reference.GetComponent<FollowCamera3D>();

        // set transform of script to spawned player object
        script.setFollowedPlayer(o.Reference);

        yield return null;
    }

    private IEnumerator DetachCamera()
    {
        DestoryData d = new DestoryData();
        d.Reference = playerCamera.Reference;
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
        playerCamera.Tag = "player-camera";


        lEvent.Raise(this.gameObject, redPlane);
        lEvent.Raise(this.gameObject, bluePlane);
        lEvent.Raise(this.gameObject, redBoat);
        lEvent.Raise(this.gameObject, blueBoat);
        lEvent.Raise(this.gameObject, playerCamera);
    }
}
