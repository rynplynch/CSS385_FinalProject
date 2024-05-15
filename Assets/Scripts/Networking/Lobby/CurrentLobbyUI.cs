using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentLobbyUI : MonoBehaviour
{
    public TestLobby testLobby;


    public TextMeshProUGUI playerCount;


    public void UpdatePlayerNumber()
    {
        playerCount.text = "";
    }
    
}
