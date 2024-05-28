using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldBounds : MonoBehaviour
{
    // game logic controller
    private GameLogic gCtrl;

    // map that holds out of bound game objects
    Dictionary<GameObject, (bool isOut, float timeLeft)> ObjectsTimeLeft =
        new Dictionary<GameObject, (bool, float)>();

    // grace period given to return to world bounds
    private float gracePeriod = 5f;

    void Start()
    {
        gCtrl = GameLogic.Instance;
    }

    void Update()
    {
        // maintain a temp dict. so we can alter time values
        Dictionary<GameObject, (bool isOut, float timeLeft)> tmp =
            new Dictionary<GameObject, (bool, float)>();
        // loop through registered objects
        foreach (var g in ObjectsTimeLeft)
        {
            // if the object is out of bound
            if (g.Value.isOut)
            {
                // extract how much time object has left
                float t = g.Value.timeLeft;

                // decrement the grace period by delta time
                t -= Time.deltaTime;

                // if the time left countdown reaches 0
                if (t < 0)
                {
                    // kill the player :c
                    gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(g.Key, 0f));

                    // skip adding this game object to temp dict.
                    break;
                }

                // save new value to temp dict.
                tmp.Add(g.Key, (true, t));
            }
        }

        // update dict.
        ObjectsTimeLeft = tmp;
    }

    void OnTriggerEnter(Collider c)
    {
        // extract game object from collider
        GameObject g = c.gameObject;

        // if the game object is a boat, plane or bot
        if (CheckTag.IsBoat(g) || CheckTag.IsPlane(g) || CheckTag.IsBot(g))
        {
            // if the player is not registered
            if (!IsRegistered(g))
            {
                // register them
                RegisterWithTimer(g);

                // break from function
                return;
            }
            // the player is inbound, reset their timer
            ObjectsTimeLeft[g] = (false, gracePeriod);

            // hide out of bounds UI
            HideOutOfBoundsUI();

            // show vehicle UI again
            ShowVehicleUI(g);
        }
    }

    void OnTriggerExit(Collider c)
    {
        // extract game object from collider
        GameObject g = c.gameObject;

        // if the game object is a boat, plane or bot
        if (CheckTag.IsBoat(g) || CheckTag.IsPlane(g) || CheckTag.IsBot(g))
        {
            // if the player is not registered
            if (!IsRegistered(g))
                // register them
                RegisterWithTimer(g);

            // the player is out of bound
            ObjectsTimeLeft[g] = (true, gracePeriod);

            // hide the vehicles UI
            HideVehicleUI(g);

            ShowOutOfBoundsUI();
        }
    }

    private void RegisterWithTimer(GameObject g)
    {
        // add object to dictionary
        ObjectsTimeLeft[g] = (false, gracePeriod);
    }

    private bool IsRegistered(GameObject g) => ObjectsTimeLeft.ContainsKey(g);

    private void ShowOutOfBoundsUI() =>
        SceneManager.LoadSceneAsync("OutOfBoundsUI", LoadSceneMode.Additive);

    private void HideOutOfBoundsUI() => SceneManager.UnloadSceneAsync("OutOfBoundsUI");

    // decide what vehicle UI to show
    private void ShowVehicleUI(GameObject o)
    {
        // if its a boat
        if (CheckTag.IsBoat(o))
            // show boat UI
            ShowBoatUIAsync();
        else if (CheckTag.IsPlane(o))
            // show plane UI
            ShowPlaneUIAsync();
    }

    // displays the boat UI
    private async void ShowBoatUIAsync()
    {
        await SceneManager.LoadSceneAsync("BoatUI", LoadSceneMode.Additive);
    }

    // displays the plane UI
    private async void ShowPlaneUIAsync()
    {
        await SceneManager.LoadSceneAsync("PlaneUI", LoadSceneMode.Additive);
    }

    // remove vehicle UI
    private void HideVehicleUI(GameObject o)
    {
        // if its a boat
        if (CheckTag.IsBoat(o))
            // show boat UI
            HideBoatUIAsync();
        else if (CheckTag.IsPlane(o))
            // show plane UI
            HidePlaneUIAsync();
    }

    // displays the boat UI
    private async void HideBoatUIAsync()
    {
        await SceneManager.UnloadSceneAsync("BoatUI");
    }

    // displays the plane UI
    private async void HidePlaneUIAsync()
    {
        await SceneManager.UnloadSceneAsync("PlaneUI");
    }

    // get how much time an object has left
    public float GetTimeLeft(GameObject o)
    {
        if (ObjectsTimeLeft.ContainsKey(o))
            return ObjectsTimeLeft[o].timeLeft;
        return 0f;
    }
}
