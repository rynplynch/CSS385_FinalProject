using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TicketSystem : SpawnListener
{
    // amount of tickets peer team
    private Dictionary<string, int> teamTickets = new Dictionary<string, int>
        {
            {"red", 500},
            {"blue", 500}
        };

    // cost of each prefab that is spawned
    private Dictionary<string, int> prefabCost = new Dictionary<string, int>
        {
            {"boat", 50},
            {"plane", 25}
        };

    // Start is called before the first frame update
    void Start()
    {
        // instantiate new unity event
        Response = new UnityEvent<GameObject, SpawnData>();

        // tells the event to call this function
        Response.AddListener(ToCall);
    }

    private void ToCall(GameObject caller, SpawnData d){
        // game object being spawned
        GameObject s = d.Prefab;

        // if the prefab is on red team
        if (CheckTag.IsRedTeam(s))
            // if prefab is a boat
            if (CheckTag.IsBoat(s))
            {
                // remove tickets from red team at cost of boat
                teamTickets["red"] = teamTickets["red"] - prefabCost["boat"];
                Debug.Log("red team tickets left: " + $"{teamTickets["red"]}");
            }
            // if prefab is a plane
            else if (CheckTag.IsPlane(s))
                // remove tickets from red team at cost of plane
                teamTickets["red"] = teamTickets["red"] - prefabCost["plane"];
        // if the prefab is on blue team
        else if (CheckTag.IsBlueTeam(s))
            // if prefab is a boat
            if (CheckTag.IsBoat(s))
                // remove tickets from blue team at cost of boat
                teamTickets["blue"] = teamTickets["blue"] - prefabCost["boat"];
            // if prefab is a plane
            else if (CheckTag.IsPlane(s))
                // remove tickets from blue team at cost of plane
                teamTickets["blue"] = teamTickets["blue"] - prefabCost["plane"];

        // if a team is out of tickets
        if (IsTeamOutOfTickets())
            // call the end game event
            GameLogic.Instance.gameOverEvent.Raise(this.gameObject, new GameOverData());
    }

    private bool IsTeamOutOfTickets()
    {
        // iterate over every teams ticket values
        foreach (int tickets in teamTickets.Values)
        {
            // if someones tickets are bellow 0
            if (tickets < 0)
                return true;
        }
        return false;
    }

    // returns number of tickets left for the specified team
    // return -1 if team is not found
    public int GetTeamTickets(string teamColor)
    {
        // if the color exists in the team color dictionary
        if (teamTickets.ContainsKey(teamColor))
            // return how many tickets they have left
            return teamTickets[teamColor];

        // if the team isn't inside the dict return -1
        return -1;
    }
}
