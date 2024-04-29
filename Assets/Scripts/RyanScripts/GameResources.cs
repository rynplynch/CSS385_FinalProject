using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private GameObject redBoatPrefab;
    // represents the prefab used to spawn the player
    public GameObject prefab
    {
        set => redBoatPrefab = value;
    }
    // public GameObject prefab;

    void Start()
    {
        loadPrefab();
    }

    public void loadPrefab(){
        Interact interactScript = this.GetComponent<Interact>();
        if (interactScript) interactScript.CallInteract(this);
    }
}
