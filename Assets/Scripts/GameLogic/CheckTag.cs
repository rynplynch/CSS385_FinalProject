using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTag : MonoBehaviour
{
    List<string> teams = new List<string>() { "red", "blue" };

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public static bool IsBoat(GameObject other)
    {
        return other.tag.Contains("boat");
    }

    public static bool IsPlane(GameObject other)
    {
        return other.CompareTag("blue-plane") || other.CompareTag("red-plane");
    }

    public static bool IsRedTeam(GameObject o) => o.tag.Contains("red");

    public static bool IsBlueTeam(GameObject o) => o.tag.Contains("blue");

    public static bool MatchingColor(string projectileTag, string targetTag)
    {
        if (projectileTag.Contains("blue") && targetTag.Contains("blue"))
        {
            return true;
        }
        else if (projectileTag.Contains("red") && targetTag.Contains("red"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
