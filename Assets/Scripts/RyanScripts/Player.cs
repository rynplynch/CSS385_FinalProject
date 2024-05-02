using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction spawnAsRedBoat;
    public InputAction spawnAsRedPlane;
    public InputAction spawnAsBlueBoat;

    // how we reference our game logic and events
    private GameLogic gCtrl;
    private SpawnEvent sEvent;
    private LoadEvent lEvent;
    private DestroyEvent dEvent;

    // objects the player can spawn as
    SpawnData redBoat = new SpawnData();
    SpawnData redPlane = new SpawnData();
    SpawnData blueBoat = new SpawnData();

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
    }

    // make sure the player can not spawn as multiple GameObjects at once
    private IEnumerator SpawnRoutine(SpawnData o){
        if (redBoat.Reference != null)
            yield return DetachCamera();
            yield return DeleteObject(redBoat);
        if (redPlane.Reference != null)
            yield return DetachCamera();
            yield return DeleteObject(redPlane);
        if (blueBoat.Reference != null)
            yield return DetachCamera();
            yield return DeleteObject(blueBoat);

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
        // destroy camera
        yield return DeleteObject(playerCamera);
    }
    private IEnumerator SpawnObject(SpawnData o)
    {
        sEvent.Raise(this.gameObject, o);
        yield return null;
    }

    private IEnumerator DeleteObject(SpawnData o)
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
        redPlane.Tag = "red-plane";
        playerCamera.Tag = "player-camera";

        lEvent.Raise(this.gameObject, redPlane);
        lEvent.Raise(this.gameObject, redBoat);
        lEvent.Raise(this.gameObject, blueBoat);
        lEvent.Raise(this.gameObject, playerCamera);
    }
}
