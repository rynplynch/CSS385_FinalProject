using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // make the GameController Singleton referable globally
    public static GameController Instance { get; private set; }

    private PlayerController playerController;

    // Start is called before the first frame update
    private void Awake()
    {
        // this will instantiate the player controller
        loadResources();

        // safety check to remove other instances of GameController
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

    // load prefabs
    private void loadResources(){
        UnityEngine.Object[] allPrefabs;

        // grab all the prefabs in the prefab folder
        try{
            allPrefabs = Resources.LoadAll("Prefabs") as UnityEngine.Object[];
        } catch(UnityException e){
            throw e;
        }

        // we will grab this reference while looping
        GameObject boatPrefab = null;

        // loop through all the Objects returned from prefab dir.
        // if more prefabs are added, add them here
        foreach (GameObject p in allPrefabs)
        {
            // based upon what the tag is do different things
            if (p.tag.Equals("PlayerController")) {
                // instantiate the player controller
                Instantiate(p, new Vector3(0,0,0), new  Quaternion());
            } else if (p.tag.Equals("Boat")){
                boatPrefab = p;
                                            }
        }

        // grab the Singleton reference to PlayerController
        playerController = PlayerController.Instance;

        // check if the playerController & boatPrefab references exist
        // if this fails then resources did not load correctly
        if (playerController && boatPrefab) {
            playerController.SetBoatPrefab(boatPrefab);
        } else {
            throw new UnityException();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
