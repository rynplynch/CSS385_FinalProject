using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradePlayer : UpgradeListener
{
    public int initialGoldCost = 50;
    private Dictionary<GameObject, int> healthLevels = new Dictionary<GameObject, int>();
    public int goldIncreasePerLevel = 100;
    private GoldManagerScript goldManager;

    void Start()
    {
        // method called when event triggered
        Response = new UnityEvent<GameObject, UpgradeData>();
        Response.AddListener(ToCall);
        goldManager = FindAnyObjectByType<GoldManagerScript>();
    }

    public void ToCall(GameObject caller, UpgradeData data)
    {
        Debug.Log(data.PlayerGold);

        return;
    }

    private void UpgradeHealth(GameObject player, int playerGold)
    {
        // if the player has never upgraded give them level 1
        if (healthLevels.ContainsKey(player))
        {
            healthLevels.Add(player, 1);
            int upgradeCost = initialGoldCost + (healthLevels[player] * goldIncreasePerLevel);
            if (upgradeCost <= playerGold)
                return;
        }
        // if (goldManager.CanAfford(playerId, upgradeCost))
        // {
        //     goldManager.SpendGold(playerId, upgradeCost);
        //     // Apply health and increase upgrade level
        //     playerHealth.maxHealth += healthIncrease;
        //     playerHealth.currentHealth += healthIncrease;
        //     healthLevels[playerId]++;
        //     UpdateUpgradeLevelText(playerId);
        // }
        else
        {
            Debug.Log("Not enough gold to upgrade health.");
        }
    }
}
