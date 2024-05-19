using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnMenu : MonoBehaviour
{
    // canvas for the entire player menu
    private Canvas canvas;

    // UI containers
    private Transform redTeamContainer;
    private Transform blueTeamContainer;

    // UI buttons
    private Button pickRed;
    private Button pickBlue;

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
        // p = g.GetComponent<Player>();

        // grab canvas of player menu
        canvas = this.gameObject.GetComponent<Canvas>();

        // grab transform of boat/plane button
        redTeamContainer = this.transform.GetChild(0);
        blueTeamContainer = this.transform.GetChild(1);

        // grab button components
        pickRed = redTeamContainer.GetComponent<Button>();
        pickBlue = blueTeamContainer.GetComponent<Button>();

        // give an action to before when the button is pushed
        pickRed.onClick.AddListener(async () =>
        {
            await SceneManager.UnloadSceneAsync("SpawnMenu");

            await SceneManager.LoadSceneAsync("RedTeamMenu", LoadSceneMode.Additive);
        });

        // give an action to before when the button is pushed
        pickBlue.onClick.AddListener(async () =>
        {
            await SceneManager.UnloadSceneAsync("SpawnMenu");

            await SceneManager.LoadSceneAsync("BlueTeamMenu", LoadSceneMode.Additive);
        });
    }
}
