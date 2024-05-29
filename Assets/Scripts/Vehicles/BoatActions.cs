using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatActions : MonoBehaviour
{
    // how we reference game logic
    private GameLogic gCtrl;

    void Start()
    {
        // grab reference
        gCtrl = GameLogic.Instance;
    }

    // when the player create an bullet upgrade event
    public void OnUpgradeBullet(InputAction.CallbackContext ctx)
    {
        // when the action is first triggered
        if (ctx.performed)
        {
            // get the vehicle component
            Vehicle v = this.GetComponent<Vehicle>();

            // which player spawned this vehicle?
            Player p = v.SpawnedBy.GetComponent<Player>();

            // only perform the upgrade if we know who spawned it
            if (p)
                // tell the upgrade system player wants to upgrade
                gCtrl.UpSystem.UpgradeBullet(p);
        }
    }

    // when the player create a health upgrade event
    public void OnUpgradeHealth(InputAction.CallbackContext ctx)
    {
        // when the action is first triggered
        if (ctx.performed)
        {
            // get the vehicle component
            Vehicle v = this.GetComponent<Vehicle>();

            // which player spawned this vehicle?
            Player p = v.SpawnedBy.GetComponent<Player>();

            // only perform the upgrade if we know who spawned it
            if (p)
                // tell the upgrade system player wants to upgrade
                gCtrl.UpSystem.UpgradeHealth(p);
        }
    }

    // when the player create a health upgrade event
    public void OnUpgradeBomb(InputAction.CallbackContext ctx)
    {
        // when the action is first triggered
        if (ctx.performed)
        {
            // get the vehicle component
            Vehicle v = this.GetComponent<Vehicle>();

            // which player spawned this vehicle?
            Player p = v.SpawnedBy.GetComponent<Player>();

            // only perform the upgrade if we know who spawned it
            if (p)
                // tell the upgrade system player wants to upgrade
                gCtrl.UpSystem.UpgradeExplosion(p);
        }
    }
}
