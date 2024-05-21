using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : DamageListener
{
    // tracks the game objects health
    // uses the game object reference as a key
    // returns tuple with current and max health
    private Dictionary<GameObject, (int CurrentHp, int MaxHp)> _objectsHealth =
        new Dictionary<GameObject, (int CurrentHp, int MaxHp)>();
    private Dictionary<GameObject, (int CurrentHp, int MaxHp)> ObjectsHealth
    {
        get => _objectsHealth;
        set => _objectsHealth = value;
    }

    // amount of gold given to a player for doing damage
    public int goldValue = 15;

    // initial values of objects that have health
    public int startingBoatHp = 1000;
    public int startinPlaneHp = 250;

    // used to invoke events
    GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        // grab game logic reference
        gCtrl = GameLogic.Instance;

        // instantiate new unity event
        Response = new UnityEvent<GameObject, DamageData>();

        // tells the event to call this function
        Response.AddListener(ToCall);
    }

    // upgrade the health of a game object
    public void UpgradeMaxHealth(GameObject o, int hpIncrease) { }

    private void ToCall(GameObject caller, DamageData d)
    {
        // the projectile doing the damage
        Projectile proj = d.Projectile.GetComponent<Projectile>();

        // we only do damage after these conditions are met
        if (
            // only projectiles can do damage
            proj
            // only do damage if we are not on the same team
            && !CheckTag.MatchingColor(d.Projectile.tag, d.ObjectToDamage.tag)
            // only do damage to boats or planes
            && (CheckTag.IsBoat(d.ObjectToDamage) || CheckTag.IsPlane(d.ObjectToDamage))
        )
        {
            // is the object we are damaging registered?
            if (!IsRegistered(d.ObjectToDamage))
                // registering the object with health system
                RegisterWithHealthSystem(d.ObjectToDamage);

            // spawn a new piece of gold
            FindFirstObjectByType<GoldManagerScript>()
                .AddGold(gCtrl.Player.Reference.GetComponent<Player>(), goldValue);

            // apply damage to the object
            ApplyDamage(d.ObjectToDamage, d.DamageToApply);

            // update all health UI elements
            Debug.Log(d.ObjectToDamage.name);
            Debug.Log("max:" + GetMaxHealth(d.ObjectToDamage));
            Debug.Log("currnt" + GetCurrentHealth(d.ObjectToDamage));
            gCtrl.updateHpUI.Invoke();

            // damage as been dealt to player, break
            return;
        }
    }

    // if an object does not exist in the objectHealth dict.
    // add them with their initial health value
    // only adds boats and plane objects
    private void RegisterWithHealthSystem(GameObject o)
    {
        // if the object is a boat
        if (CheckTag.IsBoat(o))
        {
            // set current hp and max hp of boat
            ObjectsHealth[o] = (startingBoatHp, startingBoatHp);
        }
        // if the object is a plane
        else if (CheckTag.IsPlane(o))
            // set current hp and max hp of plane
            ObjectsHealth[o] = (startinPlaneHp, startinPlaneHp);
    }

    // get an objects current health
    public int GetCurrentHealth(GameObject o)
    {
        // if the object isn't register
        if (!IsRegistered(o))
            // register it with the health system
            RegisterWithHealthSystem(o);

        return ObjectsHealth[o].CurrentHp;
    }

    // get an objects max health
    public int GetMaxHealth(GameObject o)
    {
        // if the object isn't registered
        if (!IsRegistered(o))
            // register the new object
            RegisterWithHealthSystem(o);

        return ObjectsHealth[o].MaxHp;
    }

    // set max health of object
    public void SetMaxHealth(GameObject o, int newMax)
    {
        // create new tuple, replace old data
        ObjectsHealth[o] = (GetCurrentHealth(o), newMax);
    }

    // set current health of object
    public void AddToCurrentHealth(GameObject o, int toAdd)
    {
        // because tuples are immutable we need to create a new one
        ObjectsHealth[o] = (GetCurrentHealth(o) + toAdd, GetMaxHealth(o));
    }

    // apply damage to an object
    public void ApplyDamage(GameObject o, int damage)
    {
        // make sure game object exists in dict.
        if (!ObjectsHealth.ContainsKey(o))
            RegisterWithHealthSystem(o);

        // here we subtract damage from current health
        AddToCurrentHealth(o, -damage);

        // if the objects health is bellow 0 delete it
        if (ObjectsHealth[o].CurrentHp <= 0)
        {
            // destroy the object
            gCtrl.destroyEvent.Raise(this.gameObject, new DestoryData(o, 0));

            // remove it from the dict.
            ObjectsHealth.Remove(o);
        }
    }

    // does the player exist in our dictionaries?
    private bool IsRegistered(GameObject o)
    {
        // if any of the dict. contain the player they are registered
        return _objectsHealth.ContainsKey(o);
    }
}
