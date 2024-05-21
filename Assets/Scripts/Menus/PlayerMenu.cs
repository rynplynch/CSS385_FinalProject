using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMenu : MonoBehaviour
{
    // canvas for the entire player menu
    private Canvas canvas;

    // UI containers
    private Transform clockContainer;
    private Transform blueTicketsContainer;
    private Transform redTicketsContainer;

    // actual text elements we show the player
    private TMP_Text clockUI;
    private Slider blueSlider;
    private TMP_Text blueTickets;
    private Slider redSlider;
    private TMP_Text redTickets;

    // game controller
    private GameLogic gCtrl;

    // time left in the match
    TimeSpan timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;

        // grab canvas of player menu
        canvas = this.gameObject.GetComponent<Canvas>();

        // grab transform of clock text
        clockContainer = this.transform.GetChild(0);

        // grab transform of blue/red team tickets
        blueTicketsContainer = this.transform.GetChild(1);
        redTicketsContainer = this.transform.GetChild(2);

        // extract clock text
        clockUI = clockContainer.GetComponent<TMP_Text>();

        // extract blue/red team ticket parts
        blueSlider = blueTicketsContainer.GetComponent<Slider>();
        blueTickets = blueTicketsContainer.GetChild(1).GetComponent<TMP_Text>();
        redSlider = redTicketsContainer.GetComponent<Slider>();
        redTickets = redTicketsContainer.GetChild(1).GetComponent<TMP_Text>();

        // set the ticket sliders max value
        blueSlider.maxValue = gCtrl.Ticker.StartingTickets;
        redSlider.maxValue = gCtrl.Ticker.StartingTickets;
    }

    // Update is called once per frame
    void Update()
    {
        // grab current tickets for each team
        int currBlue = gCtrl.Ticker.GetTeamTickets("blue");
        int currRed = gCtrl.Ticker.GetTeamTickets("red");

        // get the most updated time
        timeLeft = TimeSpan.FromSeconds(gCtrl.gameClock.TimeLeft);

        // set value of sliders
        blueSlider.value = currBlue;
        redSlider.value = currRed;

        // also display the amount if tickets left as text
        blueTickets.text = $"{currBlue}";
        redTickets.text = $"{currRed}";

        // display the time to the player
        clockUI.text = $"Time left: {timeLeft.Minutes}:{timeLeft.Seconds}";
    }

    // set the camera the canvas uses to render
    public void SetRenderCam(Camera c)
    {
        canvas.worldCamera = c;
    }
}
