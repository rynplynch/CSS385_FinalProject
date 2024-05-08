using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGame : GameOverListener
{
    // Start is called before the first frame update
    void Start()
    {
        // instantiate new unity event
        Response = new UnityEvent<GameObject, GameOverData>();

        // tells the event to call this function
        Response.AddListener(ToCall);
    }

    private void ToCall(GameObject caller, GameOverData d){
        GameLogic gCtrl = GameLogic.Instance;
        // run the end game routine
        StartCoroutine(gCtrl.EndGame());
    }

}
