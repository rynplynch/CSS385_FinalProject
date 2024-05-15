using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManagerScript : MonoBehaviour
{
    // Gold Tracking
    private Dictionary<int, int> playerGold = new Dictionary<int, int>(); // Store gold count for each player
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
    void Update()
    {

    }

    void SpawnGold()
    {
        if (goldSpawned < maxGoldSpawn && RandomGenerator() == true) // random spawn include y axis
        {
            Vector3 spawnPosition = new Vector3(Random.Range(min, max), Random.Range(minY, maxY), Random.Range(min, max));
            GameObject g = Instantiate(goldPrefab, spawnPosition, Quaternion.identity);
            goldSpawned++;
        }
        else if (goldSpawned < maxGoldSpawn && RandomGenerator() == false) // random spawn sea level
        {
            Vector3 spawnPosition = new Vector3(Random.Range(min, max), minY, Random.Range(min, max));
            GameObject g = Instantiate(goldPrefab, spawnPosition, Quaternion.identity);
            goldSpawned++;
        }
    }

    public void AddGold(int playerId, int goldToAdd)
    {
        if (!playerGold.ContainsKey(playerId))
            playerGold[playerId] = 0; // Initialize player's gold count if not present
        playerGold[playerId] += goldToAdd;
        goldSpawned--;
        UpdateGoldText(playerId);
    }

    // Update gold text for the specified player
    private void UpdateGoldText(int playerId)
    {
        goldTexts[playerId].text = "Player " + playerId + " Gold: " + playerGold[playerId];
    }

    // Random generator for gold spawn (air or sea)
    private bool RandomGenerator()
    {
        return Random.Range(1, 10) <= 3;
    }

    // Check if the player can afford the specified amount of gold
    public bool CanAfford(int playerId, int amount)
    {
        if (playerGold.ContainsKey(playerId))
        {
            return playerGold[playerId] >= amount;
        }
        else
        {
            Debug.LogWarning("Player " + playerId + " does not exist.");
            return false;
        }
    }

    // Spend gold for the specified player
    public void SpendGold(int playerId, int amount)
    {
        if (playerGold.ContainsKey(playerId))
        {
            playerGold[playerId] -= amount;
            UpdateGoldText(playerId);
        }
        else
        {
            Debug.LogWarning("Player " + playerId + " does not exist.");
        }
    }

    // Get the gold count for the specified player
    public int GetGold(int playerId)
    {
        if (playerGold.ContainsKey(playerId))
        {
            return playerGold[playerId];
        }
        else
        {
            Debug.LogWarning("Player " + playerId + " does not exist.");
            return 0;
        }
    }


}