using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotSystem : MonoBehaviour
{
    GameLogic gCtrl;

    // bot prefabs
    private SpawnData RedBoat { get; set; }
    private SpawnData RedPlane { get; set; }
    private SpawnData BlueBoat { get; set; }
    private SpawnData BluePlane { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // grab game logic reference
        gCtrl = GameLogic.Instance;

        gCtrl.spawnBot.AddListener(SpawnBot);
    }

    private void SpawnBot() { }

    // grab bot prefabs
    private void LoadPrefabs()
    {
        // this is how load knows which prefab to grab
        // must match tag assigned to prefab
        RedBoat.Tag = "red-boat-bot";
        BlueBoat.Tag = "blue-boat-bot";

        gCtrl.loadEvent.Raise(this.gameObject, RedBoat);
        gCtrl.loadEvent.Raise(this.gameObject, BlueBoat);
    }

    private IEnumerator SetSpawnPoint(SpawnData o)
    {
        // height vehicles are spawned
        float height = 2f;

        // construct offset vector
        Vector3 offset = GetRandomVec(spawnRadius);

        // add the offset to vehicle spawn point
        o.Position = offset;

        // if the player is spawning as a red team vehicle
        if (CheckTag.IsRedTeam(o.Prefab))
        {
            // if the player is spawning as a boat
            if (CheckTag.IsBoat(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.RedBoatSpawn.Position + offset;

                // spawn facing toward blue spawn
                o.Rotation = Quaternion.LookRotation(Vector3.right);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
            // if the player is spawning as a plane
            else if (CheckTag.IsPlane(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.RedPlaneSpawn.Position + offset;

                // spawn facing toward blue spawn
                o.Rotation = Quaternion.LookRotation(Vector3.right);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
        }
        // if the player is spawning as a red team vehicle
        else if (CheckTag.IsBlueTeam(o.Prefab))
            // if the player is spawning as a boat
            if (CheckTag.IsBoat(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.BlueBoatSpawn.Position + offset;

                // spawn facing toward red spawn
                o.Rotation = Quaternion.LookRotation(Vector3.left);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
            // if the player is spawning as a plane
            else if (CheckTag.IsPlane(o.Prefab))
            {
                // get the red spawn position and add offset
                offset = gCtrl.BluePlaneSpawn.Position + offset;

                // spawn facing toward red spawn
                o.Rotation = Quaternion.LookRotation(Vector3.left);

                // set the spawn height as constant
                o.Position = new Vector3(offset.x, height, offset.z);
            }
        yield return null;
    }

    // returns a random vector3 with the specified range
    private Vector3 GetRandomVec(int range)
    {
        // vectore to return
        Vector3 randomVec = new Vector3();

        // get random for each component
        randomVec.x = Random.Range(-range, range);
        randomVec.y = Random.Range(-range, range);
        randomVec.z = Random.Range(-range, range);

        return randomVec;
    }
}
