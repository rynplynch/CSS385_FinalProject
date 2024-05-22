using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public int initialGoldCost = 50;
    public int goldIncreasePerLevel = 100;
    public int damageIncrease = 50;
    public int boatDamageIncrease = 100;
    public int healthIncrease = 100;

    public BoatBullet boatBullet; // BoatBullet component
    public BulletBehavior bulletBehavior; // BulletBehavior component
    public MissileBehavior missileBehavior; // MissileBehavior component

    private Dictionary<Player, int> _bulletLevels = new Dictionary<Player, int>();
    public Dictionary<Player, int> BulletLevels
    {
        get => _bulletLevels;
        private set => _bulletLevels = value;
    }
    private Dictionary<Player, int> _missileLevels = new Dictionary<Player, int>();
    public Dictionary<Player, int> MissileLevels
    {
        get => _missileLevels;
        private set => _missileLevels = value;
    }
    private Dictionary<Player, int> _healthLevels = new Dictionary<Player, int>();

    private GoldManagerScript goldManager;
    private GameLogic gCtrl;

    void Start()
    {
        // grab game logic reference
        gCtrl = GameLogic.Instance;
        goldManager = FindAnyObjectByType<GoldManagerScript>();
    }

    // Update is called once per frame
    void Update() { }

    // upgrade the health level of the player
    public void UpgradeHealth(Player p)
    {
        // is the player not registered with the upgrade system?
        if (!IsRegistered(p))
        {
            // register the player with upgrade system
            RegisterWithUpgradeSystem(p);
        }
        else
        {
            // cost is based on level
            int upgradeCost = initialGoldCost + (_healthLevels[p] * goldIncreasePerLevel);

            // if the player can afford the upgrade
            if (goldManager.CanAfford(p, upgradeCost))
            {
                // subtract cost from players gold
                goldManager.SpendGold(p, upgradeCost);

                // upgrade the players health level
                _healthLevels[p]++;

                // grab the players current vehicle
                GameObject v = p.GetSpawnedVehicle();

                // if the player does have a spawned vehicle
                if (v)
                    // update the existing vehicle health
                    LvlUpVehicleHp(v);
            }
            else
            {
                Debug.Log("Not enough gold to upgrade health.");
            }
        }
    }

    public void UpgradeBullet(Player p)
    {
        // is the player not registered with the upgrade system?
        if (!IsRegistered(p))
        {
            // register the player with upgrade system
            RegisterWithUpgradeSystem(p);
        }
        else
        {
            // cost is based on level
            int upgradeCost = initialGoldCost + (_bulletLevels[p] * goldIncreasePerLevel);

            // if the player can afford the upgrade
            if (goldManager.CanAfford(p, upgradeCost))
            {
                // subtract cost from players gold
                goldManager.SpendGold(p, upgradeCost);

                // upgrade the players health level
                _bulletLevels[p]++;
            }
            else
            {
                Debug.Log("Not enough gold to upgrade bullets.");
            }
        }
    }

    public void UpgradeMissile(Player p)
    {
        // is the player not registered with the upgrade system?
        if (!IsRegistered(p))
            // register the player with upgrade system
            RegisterWithUpgradeSystem(p);
        else
        {
            // cost is based on level
            int upgradeCost = initialGoldCost + (_missileLevels[p] * goldIncreasePerLevel);

            // if the player can afford the upgrade
            if (goldManager.CanAfford(p, upgradeCost))
            {
                // subtract cost from players gold
                goldManager.SpendGold(p, upgradeCost);

                // upgrade the players missile level
                _missileLevels[p]++;
            }
            else
            {
                Debug.Log("Not enough gold to upgrade missiles.");
            }
        }
    }

    // add player to all the level dict. with a level of 0
    private void RegisterWithUpgradeSystem(Player p)
    {
        _healthLevels[p] = 0;
        _bulletLevels[p] = 0;
        _missileLevels[p] = 0;
    }

    // if the player isn't in any of the level dict.
    private bool IsRegistered(Player p)
    {
        // if any of the dict. contain the player they are registered
        return _healthLevels.ContainsKey(p)
            || _missileLevels.ContainsKey(p)
            || _bulletLevels.ContainsKey(p);
    }

    // take a new vehicle and upgrade its max hp to the players current lvl
    public void UpgradeNewVehicle(Player p, GameObject v)
    {
        // if the player is not registered
        if (!IsRegistered(p))
            // add them to system
            RegisterWithUpgradeSystem(p);

        // for every level the player has
        for (int i = 0; i < _healthLevels[p]; i++)
        {
            LvlUpVehicleHp(v);
        }
    }

    // upgrade the health of a spawned vehicle by one level
    private void LvlUpVehicleHp(GameObject v)
    {
        // get the current max hp of players vehicle
        int toApply = gCtrl.HpSystem.GetMaxHealth(v) + healthIncrease;

        // apply the health upgrade to players vehicle
        gCtrl.HpSystem.SetMaxHealth(v, toApply);
        gCtrl.HpSystem.AddToCurrentHealth(v, healthIncrease);
    }

    // returns player health lvl
    public int GetPlayerHpLvl(Player p)
    {
        // if the player is not registered
        if (!IsRegistered(p))
            // register them
            RegisterWithUpgradeSystem(p);

        // return health lvl for player
        return _healthLevels[p];
    }

    // returns player bullet lvl
    public int GetPlayerBltLvl(Player p)
    {
        // if the player is not registered
        if (!IsRegistered(p))
            // register them
            RegisterWithUpgradeSystem(p);

        // return health lvl for player
        return _bulletLevels[p];
    }

    // returns player missile lvl
    public int GetPlayerMslLvl(Player p)
    {
        // if the player is not registered
        if (!IsRegistered(p))
            // register them
            RegisterWithUpgradeSystem(p);

        // return missile lvl for player
        return _missileLevels[p];
    }

    // returns bonus damage to add to base bullet damage
    public float GetAddedBulletDamage(Player p, int dmg)
    {
        int bltLvl = GetPlayerBltLvl(p);

        // create a multiplier with players lvl
        float mlt = (float)bltLvl / 10;

        // return damage to be added to base damage
        return dmg * mlt;
    }

    // returns bonus damage to add to base missile damage
    public float GetAddedMissileDamage(Player p, int dmg)
    {
        int mslLvl = GetPlayerMslLvl(p);

        // create a multiplier with players lvl
        float mlt = (float)mslLvl / 10;

        // return damage to be added to base damage
        return dmg * mlt;
    }
}
