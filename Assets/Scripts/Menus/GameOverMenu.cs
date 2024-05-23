using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : GameOverListener
{
    // canvas for the entire player menu
    private Canvas canvas;

    // UI containers
    private Transform winnerContainer;

    // actual elements we show the player
    private TMP_Text winnerTxt;

    // Start is called before the first frame update
    void Start()
    {
        // grab canvas object
        canvas = this.gameObject.GetComponent<Canvas>();

        // grab transforms of UI elements
        winnerContainer = this.transform.GetChild(1);

        // extract UI components from their containers
        winnerTxt = winnerContainer.GetComponent<TMP_Text>();

        UpdateUI();
    }

    public void UpdateUI()
    {
        winnerTxt.text = $"{MainManager.Winner} won the game!";
    }
}
