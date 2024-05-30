using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBehavior : MonoBehaviour
{
    private int value = 100;

    //private int playerId; // Store player ID who collects the gold

    private GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
        //playerId = Random.Range(0, 4); // Assign a random player ID (assuming there are 4 players)
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        // Check if the other object is a boat or a plane
        if (CheckTag.IsBoat(o) || CheckTag.IsPlane(o))
        {
            // grab vehicle component
            Vehicle v = o.GetComponent<Vehicle>();

            // if colliding with a vehicle
            if (v)
            {
                // who spawned this vehicle?
                GameObject creator = v.SpawnedBy;

                if (creator)
                {
                    Player p = creator.GetComponent<Player>();

                    // Add gold to the player who spawned the vehicle
                    FindFirstObjectByType<GoldManagerScript>()
                        .AddGold(p, value);
                }
            }

            // Destroy the gold object
            gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(this.gameObject, 0f));
        }
    }
}
