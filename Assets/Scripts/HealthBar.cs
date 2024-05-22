using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // canvas for the hp slider
    private Canvas canvas;

    // game object of parent vehicle
    GameObject v;

    // slider elements
    private Transform hpContainer;
    private Slider hpSlider;

    // used to update slider
    private GameLogic gCtrl;

    void Start()
    {
        // use trasform to get parent vehicle
        v = this.transform.parent.gameObject;

        // grab canvas object
        canvas = this.gameObject.GetComponent<Canvas>();

        // game controller reference
        gCtrl = GameLogic.Instance;

        // grab container of slide
        hpContainer = canvas.transform.GetChild(0);

        // grab slider component
        hpSlider = hpContainer.GetComponent<Slider>();

        gCtrl.updateHpUI.AddListener(UpdateUI);
    }

    public void UpdateUI()
    {
        float currHp = gCtrl.HpSystem.GetCurrentHealth(v);
        int maxHp = gCtrl.HpSystem.GetMaxHealth(v);

        hpSlider.maxValue = maxHp;
        hpSlider.value = currHp;
    }
}
