using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManagerScript : MonoBehaviour
{
    // Gold Tracking
    private Dictionary<Player, int> playerGold = new Dictionary<Player, int>(); // Store gold count for each player
    public TMP_Text[] goldTexts; // Array of TMP_Text for displaying gold count of each player

    public GameObject goldPrefab;
    public int maxGoldSpawn; // Maximum number of gold to spawn in air
    public float spawnInterval; // Interval between gold spawns

    // Gold spawn boundaries
    private float min = -400.0f;
    private float max = 400.0f;
    private float minY = 4f;
    private float maxY = 60f;

    private int goldSpawned = 0; // Counter for spawned gold

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnGold), 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update() { }

    void SpawnGold()
    {
        if (goldSpawned < maxGoldSpawn && RandomGenerator() == true) // random spawn include y axis
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(min, max),
                Random.Range(minY, maxY),
                Random.Range(min, max)
            );
            GameObject g = Instantiate(goldPrefab, spawnPosition, Quaternion.identity);
            goldSpawned++;
        }
        else if (goldSpawned < maxGoldSpawn && RandomGenerator() == false) // random spawn sea level
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(min, max),
                minY,
                Random.Range(min, max)
            );
            GameObject g = Instantiate(goldPrefab, spawnPosition, Quaternion.identity);
            goldSpawned++;
        }
    }

    public void AddGold(Player p, int goldToAdd)
    {
        // of given a null value return
        if (!p)
            return;

        // if the player is not registered
        if (!IsRegistered(p))
            RegisterPlayer(p);

        playerGold[p] += goldToAdd;
        goldSpawned--;
    }

    // Random generator for gold spawn (air or sea)
    private bool RandomGenerator()
    {
        return Random.Range(1, 10) <= 3;
    }

    // Check if the player can afford the specified amount of gold
    public bool CanAfford(Player p, int amount)
    {
        if (playerGold.ContainsKey(p))
        {
            return playerGold[p] >= amount;
        }
        else
        {
            Debug.LogWarning("Player " + p + " does not exist.");
            return false;
        }
    }

    // Spend gold for the specified player
    public void SpendGold(Player p, int amount)
    {
        if (playerGold.ContainsKey(p))
        {
            playerGold[p] -= amount;
            // UpdateGoldText(playerId);
        }
        else
        {
            Debug.LogWarning("Player " + p + " does not exist.");
        }
    }

    // Get the gold count for the specified player
    public int GetGold(Player p)
    {
        if (playerGold.ContainsKey(p))
        {
            return playerGold[p];
        }
        else
        {
            RegisterPlayer(p);
            return 0;
        }
    }

    // register a new player with the gold manager
    private void RegisterPlayer(Player p)
    {
        playerGold.Add(p, 0);
    }

    // is the player registered with the gold manager?
    private bool IsRegistered(Player p) => p && playerGold.ContainsKey(p);
}
