using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaneUI : MonoBehaviour
{
    // canvas for the entire player menu
    private Canvas canvas;

    // UI containers
    private Transform goldContainer;
    private Transform hpContainer;
    private Transform helpContainer;
    private Transform hpLvlContainer;
    private Transform bltLvlContainer;
    private Transform mslLvlContainer;

    // actual text elements we show the player
    private Slider hpSlider;
    private TMP_Text goldText;
    private TMP_Text hpText;
    private Button helpBtn;
    private TMP_Text hpLvlTxt;
    private TMP_Text bltLvlTxt;
    private TMP_Text mslLvlTxt;

    // game controller
    private GameLogic gCtrl;
    private GoldManagerScript goldManager;

    // Start is called before the first frame update
    void Start()
    {
        // reference to gold manager
        goldManager = FindAnyObjectByType<GoldManagerScript>();

        // game controller reference
        gCtrl = GameLogic.Instance;

        // grab canvas object
        canvas = this.gameObject.GetComponent<Canvas>();

        // grab transforms of UI elements
        goldContainer = this.transform.GetChild(0);
        hpContainer = this.transform.GetChild(1);
        helpContainer = this.transform.GetChild(2);
        hpLvlContainer = this.transform.GetChild(3);
        bltLvlContainer = this.transform.GetChild(4);
        mslLvlContainer = this.transform.GetChild(5);

        // extract UI components from their containers
        goldText = goldContainer.GetComponent<TMP_Text>();
        hpSlider = hpContainer.GetComponent<Slider>();
        hpText = hpContainer.GetChild(1).GetComponent<TMP_Text>();
        helpBtn = helpContainer.GetComponent<Button>();
        hpLvlTxt = hpLvlContainer.GetComponent<TMP_Text>();
        bltLvlTxt = bltLvlContainer.GetComponent<TMP_Text>();
        mslLvlTxt = mslLvlContainer.GetComponent<TMP_Text>();

        // set the hp sliders max value
        hpSlider.maxValue = 1000;
    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // get the player that created this boat
        Player p = gCtrl.Player.Reference.GetComponent<Player>();

        // get data we need to present to user
        int curHp = gCtrl.HpSystem.GetCurrentHealth(p.GetSpawnedVehicle());
        int maxHp = gCtrl.HpSystem.GetMaxHealth(p.GetSpawnedVehicle());
        int hpLvl = gCtrl.UpSystem.GetPlayerHpLvl(p);
        int bltLvl = gCtrl.UpSystem.GetPlayerBltLvl(p);
        int mslLvl = gCtrl.UpSystem.GetPlayerMslLvl(p);
        int playerGold = goldManager.GetGold(p);

        // apply data values to UI elements
        hpSlider.value = curHp;
        hpSlider.maxValue = maxHp;
        hpText.text = $"HP:{curHp}";
        hpLvlTxt.text = $"HP lvl:{hpLvl}";
        bltLvlTxt.text = $"Bullet lvl:{bltLvl}";
        mslLvlTxt.text = $"Missile lvl:{mslLvl}";
        goldText.text = $"Player gold:{playerGold}";
    }

    // set the camera the canvas uses to render
    public void SetRenderCam(Camera c)
    {
        canvas.worldCamera = c;
    }

    // event created by the player
    public void OnShowPlayerMenu(InputAction.CallbackContext ctx)
    {
        // when the show player menu action is performed
        if (ctx.performed)
        {
            helpBtn.interactable = false;
            // load the player menu scene
            SceneManager.LoadScene("PlayerMenu", LoadSceneMode.Additive);
        }
        // when the show player menu action stops
        else if (ctx.canceled)
        {
            helpBtn.interactable = true;
            // remove the player menu
            SceneManager.UnloadSceneAsync("PlayerMenu");
        }
    }
}
