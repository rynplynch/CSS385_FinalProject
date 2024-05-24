using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotSystem : MonoBehaviour
{
    GameLogic gCtrl;

    // bot spawn radius
    int spawnRadius = 200;

    // how often bots are spawned
    int spawnInterval = 3;

    // how many bots are spawned
    int numRedBot = 0;
    int numBlueBot = 0;

    // how many bots are allowed to spawn
    int botCap = 12;

    // bot prefabs
    private SpawnData RedBoat { get; set; }
    private SpawnData RedPlane { get; set; }
    private SpawnData BlueBoat { get; set; }
    private SpawnData BluePlane { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        RedBoat = new SpawnData();
        RedPlane = new SpawnData();
        BlueBoat = new SpawnData();
        BluePlane = new SpawnData();

        // grab game logic reference
        gCtrl = GameLogic.Instance;

        LoadPrefabs();

        gCtrl.spawnBot.AddListener(SpawnBlueBot);
        gCtrl.spawnBot.AddListener(SpawnRedBot);

        InvokeRepeating(nameof(SpawnBlueBot), 1f, spawnInterval);
        InvokeRepeating(nameof(SpawnRedBot), 1f, spawnInterval);
    }

    private void SpawnBlueBot()
    {
        // check if blue team has reach boat cap
        if (numBlueBot <= botCap && Random.Range(1, 10) <= 5)
        {
            // set boat spawn point
            SetSpawnPoint(BlueBoat);

            // spawn boat
            gCtrl.spawnEvent.Raise(this.gameObject, BlueBoat);

            // increment blue team boat counter
            numBlueBot++;
        }
        else if (numBlueBot <= botCap)
        {
            // set boat spawn point
            SetSpawnPoint(BluePlane);

            // spawn boat
            gCtrl.spawnEvent.Raise(this.gameObject, BluePlane);

            // increment blue team boat counter
            numBlueBot++;
        }
    }

    private void SpawnRedBot()
    {
        // check if the red team has reach boat cap
        if (numRedBot <= botCap && Random.Range(1, 10) <= 5)
        {
            // set boat spawn point
            SetSpawnPoint(RedBoat);

            // spawn boat
            gCtrl.spawnEvent.Raise(this.gameObject, RedBoat);

            // increment red team boat counter
            numRedBot++;
        }
        else if (numRedBot <= botCap)
        {
            // set boat spawn point
            SetSpawnPoint(RedPlane);

            // spawn boat
            gCtrl.spawnEvent.Raise(this.gameObject, RedPlane);

            // increment red team boat counter
            numRedBot++;
        }
    }

    // grab bot prefabs
    private void LoadPrefabs()
    {
        // this is how load knows which prefab to grab
        // must match tag assigned to prefab
        RedBoat.Tag = "red-boat-bot";
        RedPlane.Tag = "red-plane-bot";
        BlueBoat.Tag = "blue-boat-bot";
        BluePlane.Tag = "blue-plane-bot";

        gCtrl.loadEvent.Raise(this.gameObject, RedBoat);
        gCtrl.loadEvent.Raise(this.gameObject, RedPlane);
        gCtrl.loadEvent.Raise(this.gameObject, BlueBoat);
        gCtrl.loadEvent.Raise(this.gameObject, BluePlane);
    }

    private void SetSpawnPoint(SpawnData o)
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

    // reduce number of spawned red team boats
    public void ReduceNumRedBoat() => numRedBot--;

    // reduce number of spawned blue team boats
    public void ReduceNumBlueBoat() => numBlueBot--;
}
