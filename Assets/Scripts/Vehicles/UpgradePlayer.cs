using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradePlayer : UpgradeListener
{
    public int initialGoldCost = 50;
    public int healthIncrease = 100;
    public int goldIncreasePerLevel = 100;

    private Dictionary<GameObject, int> healthLevels = new Dictionary<GameObject, int>();

    void Start()
    {
        // method called when event triggered
        Response = new UnityEvent<GameObject, UpgradeData>();
        Response.AddListener(ToCall);
    }

    public void ToCall(GameObject caller, UpgradeData data)
    {
        // if there is a player health component upgrade it
        if (data.PlayerHealth != null)
            UpgradeHealth(data);

        return;
    }

    public int GetPlayerHealthLvl(GameObject player)
    {
        // if the player exists in the dict.
        if (healthLevels.ContainsKey(player))
            // return the health level of the player
            return healthLevels[player];
        else
        {
            // add the player to the dict.
            healthLevels.Add(player, 0);

            // return a lvl of 0
            return 0;
        }
    }

    private void UpgradeHealth(UpgradeData data)
    {
        // if the player does not exist
        if (!healthLevels.ContainsKey(data.Player))
        {
            // add them to the health level dict.
            healthLevels.Add(data.Player, 1);

            // if we upgrade how much does it cost?
            int upgradeCost = initialGoldCost + (0 * goldIncreasePerLevel);

            // if the cost is greater than the players gold return
            if (upgradeCost > data.PlayerGold)
                return;

            // increase max health of health component
            data.PlayerHealth.maxHealth += healthIncrease;
        }
        else
        {
            // if we upgrade how much does it cost?
            int upgradeCost = initialGoldCost + (healthLevels[data.Player] * goldIncreasePerLevel);

            // if the cost is greater than the players gold return
            if (upgradeCost > data.PlayerGold)
                return;

            // increase max health of health component
            data.PlayerHealth.maxHealth += healthIncrease;

            // increase health level
            healthLevels[data.Player]++;
        }
    }
}
