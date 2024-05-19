using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RedTeamMenu : MonoBehaviour
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

        // give an action to before when the button is pushed
        spawnBoat.onClick.AddListener(async () =>
        {
            // unload this menu
            await SceneManager.UnloadSceneAsync("RedTeamMenu");

            // spawn a red boat
            p.SpawnSelection("redBoat");
        });

        // give an action to before when the button is pushed
        spawnPlane.onClick.AddListener(async () =>
        {
            await SceneManager.UnloadSceneAsync("RedTeamMenu");

            p.SpawnSelection("redPlane");
        });
    }
}
