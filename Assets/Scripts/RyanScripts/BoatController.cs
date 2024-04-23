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
    private GameObject boatPrefab;

    // used to keep track of instantiated GameObjects
    private List<GameObject> players = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (spawn.WasPerformedThisFrame()){
            // spawn player at origin w/ rotation info
            SpawnPlayer(new Vector3(), new Quaternion());
        }
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
    }

    // spawn the player at the specified vector
    public void SpawnPlayer(Vector3 spawnPos, Quaternion spawnRot){
        Instantiate(boatPrefab, spawnPos, spawnRot);
    }

    public bool DeletePlayer(GameObject player){
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
    public void SetBoatPrefab(GameObject p) => boatPrefab = p;
}