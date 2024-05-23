using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGame : GameOverListener
{
    GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
        // instantiate new unity event
        Response = new UnityEvent<GameObject, GameOverData>();

        // tells the event to call this function
        Response.AddListener(ToCall);
    }

    // Update is called once per frame
    private void ToCall(GameObject caller, GameOverData d)
    {
        // run the end game routine
        StartCoroutine(gCtrl.EndGame());
    }
}
