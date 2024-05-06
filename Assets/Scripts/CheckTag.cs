using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool IsBoat(Collider other)
    {
        return other.CompareTag("blue-boat") || other.CompareTag("red-boat");
    }

    public static bool IsPlane(Collider other)
    {
        return other.CompareTag("blue-plane") || other.CompareTag("red-plane");
    }

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
