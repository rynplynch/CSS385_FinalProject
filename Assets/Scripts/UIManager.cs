using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject spawnUI;
    public Player player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        spawnUI.SetActive(true);
    }

    public void SelectTeamAndVehicle(string teamVehicle)
    {
        player = FindFirstObjectByType<Player>();
        player.SpawnSelection(teamVehicle);
        spawnUI.SetActive(false);
    }
}
