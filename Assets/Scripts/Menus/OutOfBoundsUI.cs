using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutOfBoundsUI : MonoBehaviour
{
    // canvas for the entire player menu
    private Canvas canvas;

    // UI containers
    private Transform clockContainer;

    // actual text elements we show the player
    private TMP_Text clockUI;

    // game logic
    WorldBounds wb;

    GameLogic gCtrl;

    void Start()
    {
        gCtrl = GameLogic.Instance;
        wb = FindAnyObjectByType<WorldBounds>();

        // grab canvas of player menu
        canvas = this.gameObject.GetComponent<Canvas>();

        // grab transform of clock text
        clockContainer = this.transform.GetChild(0);

        // extract clock text
        clockUI = clockContainer.GetComponent<TMP_Text>();
    }

    void Update()
    {
        // get the player that created this boat
        Player p = gCtrl.Player.Reference.GetComponent<Player>();

        // only update text if player exists
        if (p && p.GetSpawnedVehicle())
            clockUI.text = $"{wb.GetTimeLeft(p.GetSpawnedVehicle())}";
    }
}
