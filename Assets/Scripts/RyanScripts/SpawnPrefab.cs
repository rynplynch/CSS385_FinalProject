using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    // delegate used to subsribe to our event system
    public EventSystem spawnFromInteraction;

    void Start(){
    }

    private void OnEnable()
    {
        EventSystem check = GetComponent<EventSystem>();

        if (check)
        {
            spawnFromInteraction = check;
            spawnFromInteraction.GetSpawnEvent.HasInteracted += InteractSpawn;
        }
        else
        {
            EventSystem addComp = this.gameObject.AddComponent<EventSystem>();
            spawnFromInteraction = addComp;
            spawnFromInteraction.GetSpawnEvent.HasInteracted += InteractSpawn;
        }
    }

    private void OnDisable()
    {
        if (spawnFromInteraction)
        {
            spawnFromInteraction.GetSpawnEvent.HasInteracted -= InteractSpawn;
        }
    }

    public void InteractSpawn()
    {
        SpawnRedBoatPrefab(spawnFromInteraction.GameLogic);
    }

    public void SpawnRedBoatPrefab(GameLogic resources){
        Instantiate(resources.RedBoat, resources.RedSpawn, new Quaternion());
    }
}
