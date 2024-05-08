using UnityEngine;
using UnityEngine.Events;

public class LoadResources : LoadListener
{
    // list with all Objects in resource folder
    UnityEngine.Object[] allPrefabs;

    void Start(){
        // grab all the prefabs in the prefab folder
        try{
            allPrefabs = Resources.LoadAll("Prefabs") as UnityEngine.Object[];
        } catch(UnityException e){
            throw e;
        }

        // instantiate new unity event
        Response = new UnityEvent<GameObject, SpawnData>();

        // tells the event to call this function
        Response.AddListener(FindPrefab);
    }

    public void FindPrefab(GameObject caller, SpawnData d){
        // safety check for cast success
        if (d != null)
        {
            // loop until we find a matching tag
            foreach (GameObject p in allPrefabs)
            {
                if (p.tag.Equals(d.Tag))
                {
                    d.Prefab = p;
                    d.Position = p.transform.position;
                    d.Rotation = p.transform.rotation;
                    return;
                }
            }
        }
    }
}