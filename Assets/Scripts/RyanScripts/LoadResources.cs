using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadResources : MonoBehaviour
{
    // delegate used to subsribe to our event system
    public Interact loadFromInteraction;

    private void OnEnable()
    {
        Interact check = GetComponent<Interact>();

        if (check)
        {
            loadFromInteraction = check;
            loadFromInteraction.GetInteractEvent.HasInteracted += InteractLoad;
        }
        else
        {
            Interact addComp = this.gameObject.AddComponent<Interact>();
            loadFromInteraction = addComp;
            loadFromInteraction.GetInteractEvent.HasInteracted += InteractLoad;
        }
    }

    private void OnDisable()
    {
        if (loadFromInteraction)
        {
            loadFromInteraction.GetInteractEvent.HasInteracted -= InteractLoad;
        }
    }

    public void InteractLoad()
    {
        LoadPlayerPrefab(loadFromInteraction.GetResources);
    }

    public void LoadPlayerPrefab(GameResources player){
        // load the resource
        Debug.Log("Hello");
        player.prefab = new GameObject();
    }
}
