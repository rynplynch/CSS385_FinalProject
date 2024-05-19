using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMenu : MonoBehaviour
{
    // canvas for the entire player menu
    private Canvas canvas;

    // UI containers
    private Transform clockContainer;

    // actual text elements we show the player
    private TMP_Text clockUI;

    // Start is called before the first frame update
    void Start()
    {
        // grab canvas of player menu
        canvas = this.gameObject.GetComponent<Canvas>();

        // grab transform of clock text
        clockContainer = this.transform.GetChild(0);

        // extract clock text
        clockUI = clockContainer.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        clockUI.text = Random.Range(0, 10).ToString();
    }

    // set the camera the canvas uses to render
    public void SetRenderCam(Camera c)
    {
        canvas.worldCamera = c;
    }
}
