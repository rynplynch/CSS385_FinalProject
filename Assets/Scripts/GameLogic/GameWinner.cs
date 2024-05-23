using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinner : MonoBehaviour
{
    GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
    }

    // returns the current winner of the game
    public string GetGameWinner()
    {
        return gCtrl.Ticker.GetLeastTicket();
    }
}
