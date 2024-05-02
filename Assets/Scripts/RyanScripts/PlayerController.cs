using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // make the PlayerController Singleton referable globally
    public static PlayerController Instance { get; private set; }

    // used to create new players
    public InputAction spawn;
    public InputAction spawnAsPlane;
    public InputAction spawnAsBoat;
    public InputAction despawnAll;
    private GameObject boatPrefab;
    private GameObject planePrefab;
    private GameObject planeHelper;
    private FollowCamera3D cam;
    private GameObject currentPlane;

    // used to keep track of instantiated GameObjects
    private List<GameObject> players = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (spawn.WasPerformedThisFrame()){
            // spawn player at origin w/ rotation info
            SpawnPlayer(new Vector3(), new Quaternion());
        }
        if (spawnAsBoat.WasPerformedThisFrame())
            spawnPlayerAsBoat(new Vector3(), new Quaternion());
        if (spawnAsPlane.WasPerformedThisFrame())
            spawnPlayerAsPlane(new Vector3(), new Quaternion());
    }

    void Start(){
    }

    // Start is called before the first frame update
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

        spawn.Enable();
        spawnAsBoat.Enable();
        spawnAsPlane.Enable();
        despawnAll.Enable();
    }

    // spawn the player at the specified vector
    public void SpawnPlayer(Vector3 spawnPos, Quaternion spawnRot){
        players.Add(Instantiate(boatPrefab, spawnPos, spawnRot));
    }

    // spawn as a boat
    public void spawnPlayerAsBoat(Vector3 spawnPos, Quaternion spawnRot){
        deletePlayer(currentPlane);
        // create the new player
        GameObject newplayer = Instantiate(boatPrefab, spawnPos, spawnRot);

        // add them to instantiated players list
        players.Add(newplayer);

        // if the camera was following someone delete them
        deletePlayer(cam.getFollowedPlayer());

        // tell the camera to follow new player
        cam.setFollowedPlayer(newplayer);
    }

    public void spawnPlayerAsPlane(Vector3 spawnPos, Quaternion spawnRot){
        // if the camera was following someone else delete them
        deletePlayer(cam.getFollowedPlayer());
        deletePlayer(currentPlane);
        // create the new player
        GameObject newPlayer = Instantiate(planePrefab, spawnPos, spawnRot);
        currentPlane = newPlayer;

        // plane also has a helper spawn in-front of plane
        GameObject newHelper = Instantiate(planeHelper, spawnPos + new Vector3(0,0,10), spawnRot);

        // planes movement script needs reference to PlaneHelper
        PlayerMovement pm = newPlayer.GetComponent<PlayerMovement>();
        pm.mouseTarget = newHelper.transform.GetChild(0).gameObject;

        // planeHelper needs transform of player body
        MouseMove mouse = newHelper.GetComponent<MouseMove>();
        mouse.playerBody = newPlayer.transform;

        // add them to instantiated players list
        players.Add(newPlayer);
        players.Add(newHelper);

        cam.setFollowedPlayer(newHelper);
    }

    public bool deletePlayer(GameObject player){
        // remove the player from our list of instantiated objects
        if (players.Remove(player)){
            // remove object from game world
            Destroy(player);

            // action succeeded
            return true;
        }

        return false;
    }
    // set the reference to the boat prefab we use
    // should be set when the player controller is loaded in
    public void setBoatPrefab(GameObject p) => boatPrefab = p;

    public void setPlanePrefab(GameObject p) => planePrefab = p;

    public void setPlaneHelper(GameObject p) => planeHelper = p;
}
