using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public int initialGoldCost = 50; 
    public int goldIncreasePerLevel = 100; 
    public int damageIncrease = 50; 
    public int healthIncrease = 100;

    public BulletBehavior bulletBehavior; // BulletBehavior component
    public MissileBehavior missileBehavior; // MissileBehavior component
    public PlayerHealth playerHealth; // PlayerHealth component

    private Dictionary<int, int> bulletLevels = new Dictionary<int, int>(); 
    private Dictionary<int, int> missileLevels = new Dictionary<int, int>();
    private Dictionary<int, int> healthLevels = new Dictionary<int, int>(); 

    private GoldManagerScript goldManager;

    // upgrade manager instances
    private static UpgradeManager _instance;
    public static UpgradeManager Instance { get { return _instance; } }

    // Display levels of upgrades
    public TextMeshProUGUI bulletLevelText;
    public TextMeshProUGUI missileLevelText;
    public TextMeshProUGUI healthLevelText;

    void Start()
    {
        goldManager = FindAnyObjectByType<GoldManagerScript>();
        InitializeLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeLevels()
    {
        // Initialize bullet and missile levels for each player
        for (int i = 0; i < goldManager.goldTexts.Length; i++)
        {
            bulletLevels[i] = 0;
            missileLevels[i] = 0;
            healthLevels[1] = 0;
        }
    }

    private void Awake()
    {
        // Ensure there's only one instance of UpgradeManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    public void UpgradeHealth(int playerId)
    {
        int upgradeCost = initialGoldCost + (healthLevels[playerId] * goldIncreasePerLevel);
        if (goldManager.CanAfford(playerId, upgradeCost))
        {
            goldManager.SpendGold(playerId, upgradeCost);
            // Apply health and increase upgrade level
            playerHealth.maxHealth += healthIncrease;
            playerHealth.currentHealth += healthIncrease;
            healthLevels[playerId]++;
            UpdateUpgradeLevelText(playerId);
        }
        else
        {
            Debug.Log("Not enough gold to upgrade health.");
        }
    }

    public void UpgradeBullet(int playerId)
    {
        int upgradeCost = initialGoldCost + (bulletLevels[playerId] * goldIncreasePerLevel);
        if (goldManager.CanAfford(playerId, upgradeCost))
        {
            goldManager.SpendGold(playerId, upgradeCost);
            // Apply bullet and increase upgrade level
            bulletBehavior.boatDamage += damageIncrease;
            bulletBehavior.planeDamage += damageIncrease;
            bulletLevels[playerId]++;
            UpdateUpgradeLevelText(playerId);
        }
        else
        {
            Debug.Log("Not enough gold to upgrade bullet damage.");
        }
    }

    public void UpgradeMissile(int playerId)
    {
        int upgradeCost = initialGoldCost + (missileLevels[playerId] * goldIncreasePerLevel);
        if (goldManager.CanAfford(playerId, upgradeCost))
        {
            goldManager.SpendGold(playerId, upgradeCost);
            // Apply missile and increase upgrade level
            missileBehavior.boatDamage += damageIncrease;
            missileBehavior.planeDamage += damageIncrease;
            missileLevels[playerId]++;
            UpdateUpgradeLevelText(playerId);
        }
        else
        {
            Debug.Log("Not enough gold to upgrade missile damage.");
        }
    }

    // Method to update UI text for upgrade levels
    public void UpdateUpgradeLevelText(int playerId)
    {
        // Update health level text
        if (healthLevelText != null)
        {
            healthLevelText.text = "Health Level: " + healthLevels[playerId];
        }
        
        // Update bullet level text
        if (bulletLevelText != null)
        {
            //bulletLevelText.text = "Bullet Level: " + GetBulletLevel(playerId).ToString();
            bulletLevelText.text = "Bullet Level: " + bulletLevels[playerId];
        }

        // Update missile level text
        if (missileLevelText != null)
        {
            missileLevelText.text = "Missile Level: " + missileLevels[playerId];
        }
    }
}
