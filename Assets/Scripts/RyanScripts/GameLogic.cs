using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this class should be the only object in our hierarchy
public class GameLogic : MonoBehaviour
{
    private LoadResources resources;
    private SpawnPrefab spawner;
    private EventSystem eventSystem;

    private GameObject redBoat;
    public GameObject RedBoat{ get => redBoat; set => redBoat = value; }
    private Vector3 redSpawn;
    public Vector3 RedSpawn{ get => redSpawn; set => redSpawn = value; }
    public InputAction spawn;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = this.gameObject.AddComponent<EventSystem>();
        // how we will manage the resources used in our project
        resources = this.gameObject.AddComponent<LoadResources>();
        spawner = this.gameObject.AddComponent<SpawnPrefab>();
        // how we will instantiate new GameObjects in world space
        // spawner = this.gameObject.AddComponent<SpawnPrefab>();

        RedSpawn = new Vector3(0,0,0);

        spawn.Enable();
        // eventSystem.CallInteract(resources);
    }

    // Update is called once per frame
    void Update()
    {
        if(redBoat == null){
            eventSystem.CallLoad(this);
        }

        if(spawn.WasPerformedThisFrame()) eventSystem.CallSpawn(this);
        // if(!resources) resources = this.gameObject.AddComponent<LoadResources>();
        // if(resources && !eventSystem) this.gameObject.AddComponent<EventSystem>();
        // if(eventSystem && !redBoat) eventSystem.CallLoad(redBoat, "red-boat");
    }
}
