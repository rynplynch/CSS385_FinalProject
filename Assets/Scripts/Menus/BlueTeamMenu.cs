using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlueTeamMenu : MonoBehaviour
{
    // canvas for the entire player menu
    private Canvas canvas;

    // UI containers
    private Transform boatContainer;
    private Transform planeContainer;

    // UI buttons
    private Button spawnBoat;
    private Button spawnPlane;

    // used to spawn new boats/planes
    private Player p;
    private GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;

        // grab player reference
        GameObject g = gCtrl.Player.Reference;

        // grab player script
        p = g.GetComponent<Player>();

        // grab canvas of player menu
        canvas = this.gameObject.GetComponent<Canvas>();

        // grab transform of boat/plane button
        boatContainer = this.transform.GetChild(0);
        planeContainer = this.transform.GetChild(1);

        // grab button components
        spawnBoat = boatContainer.GetComponent<Button>();
        spawnPlane = planeContainer.GetComponent<Button>();

        // give an action for when the button is pushed
        spawnBoat.onClick.AddListener(async () =>
        {
            // unload this menu
            await SceneManager.UnloadSceneAsync("BlueTeamMenu");

            // spawn a red boat
            p.SpawnSelection("blueBoat");
        });

        // give an action for when the button is pushed
        spawnPlane.onClick.AddListener(async () =>
        {
            await SceneManager.UnloadSceneAsync("BlueTeamMenu");

            p.SpawnSelection("bluePlane");
        });
    }
}
