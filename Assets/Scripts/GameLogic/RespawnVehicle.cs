using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnVehicle : DestroyListener
{
    void Start()
    {
        // method called when event triggered
        Response = new UnityEvent<GameObject, DestoryData>();
        Response.AddListener(RespawnPlayer);
        GameLogic gCtrl = GameLogic.Instance;
    }

    public void RespawnPlayer(GameObject caller, DestoryData data)
    {
        // if the prefab we are destroying has a vehicle component
        Vehicle v = data.Reference.GetComponent<Vehicle>();


        if (v)
        {
            Debug.Log("HEELE?");
            // grab reference to player that spawned vehicle
            Player p = v.SpawnedBy;
        }
    }
}
