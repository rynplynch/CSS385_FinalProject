using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class PlayerList : MonoBehaviour
{
    public List<Unity.Services.Lobbies.Models.Player> playerList;


    void Start()
    {
        playerList = new List<Unity.Services.Lobbies.Models.Player>();
    }


    public void updateList(List<Unity.Services.Lobbies.Models.Player> players)
    {
        playerList = players;
    }


}
