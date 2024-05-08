using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    // property that tracks how much time remains in the game
    public float TimeLeft { get; private set; }

    // how long is the game? Value to count down from
    public float GameDuration { get; set; }

    // is the clock counting down?
    public bool IsCounting { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // default values
        // set game length in minutes
        SetGameLengthMins(10);

        // set timeleft equal to game duration
        ResetClock();

        // don't start the count down yet
        IsCounting = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if the clock is counting down
        if (IsCounting){
            // remove the time passed since last updated
            TimeLeft = TimeLeft - Time.deltaTime;

            // if time left is less than 0 the game is over
            if (TimeLeft <= 0)
                // raise the end game event
                GameLogic.Instance.gameOverEvent.Raise(this.gameObject, new GameOverData());
        }
    }

    // restart the game clock
    public void ResetClock() => TimeLeft = GameDuration;

    // how many minutes should games last?
    public void SetGameLengthMins(int minutes) => GameDuration = (float) (minutes * 60);
}
