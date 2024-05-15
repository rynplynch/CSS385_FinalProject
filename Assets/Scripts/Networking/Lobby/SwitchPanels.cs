using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchPanels : MonoBehaviour
{
    public GameObject[] panels;
    [SerializeField]
    private int startingPanel;

    void Start()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        panels[startingPanel].SetActive(true);
    }

    //ID of 0 is the find lobby panel
    //ID of 1 is the create lobby panel
    //ID of 2 is the panel of the lobby you just joined
    public void SwitchPanel(int panelID)
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        panels[panelID].SetActive(true);
    }

}
