using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadResources : MonoBehaviour
{

    // delegate used to subsribe to our event system
    public EventSystem loadFromEventSystem;

    // list with all Objects in resource folder
    UnityEngine.Object[] allPrefabs;


    void Start(){
        // grab all the prefabs in the prefab folder
        try{
            allPrefabs = Resources.LoadAll("Prefabs") as UnityEngine.Object[];
        } catch(UnityException e){
            throw e;
        }
    }

    private void OnEnable()
    {
        EventSystem check = GetComponent<EventSystem>();

        if (check)
        {
            loadFromEventSystem = check;
            loadFromEventSystem.GetLoadEvent.HasInteracted += EventSystemLoad;
        }
        else
        {
            EventSystem addComp = this.gameObject.AddComponent<EventSystem>();
            loadFromEventSystem = addComp;
            loadFromEventSystem.GetLoadEvent.HasInteracted += EventSystemLoad;
        }
    }

    private void OnDisable()
    {
        if (loadFromEventSystem)
        {
            loadFromEventSystem.GetLoadEvent.HasInteracted -= EventSystemLoad;
        }
    }

    public void EventSystemLoad()
    {
        LoadRedBoatPrefab(loadFromEventSystem.GameLogic);
    }

    public void LoadRedBoatPrefab(GameLogic gameLogic){
        // prefab reference to be set by loop
        foreach (GameObject p in allPrefabs)
        {
            if (p.tag.Equals("red-boat")){
                gameLogic.RedBoat = p;
                return;
            }
        }
    }
}
