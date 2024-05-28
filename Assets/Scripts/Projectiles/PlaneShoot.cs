using System.Collections;
using System.Collections.Generic;
//using Unity.Play.Publisher.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public float bulletSpeed;
    public float missileSpeed;
    public float bulletCooldown;
    public float missileCooldown;
    public float bulletLifeTime; // Amount of time passes before destroy self
    public float missileLifeTime; // Amount of time passes before destroy self

    // used to invoke spawn event
    private GameLogic gCtrl;
    private float bulletNextFireTime;
    private float missileNextFireTime;

    // is the respective action being performed?
    private bool isPrimaryFireActive = false;
    private bool isSecondaryFireActive = false;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // if primary fire active and  bullet cool down is done
        if (isPrimaryFireActive && Time.time >= bulletNextFireTime)
            PrimaryFire();

        // if secondary fire active and missile cool down is done
        if (isSecondaryFireActive && Time.time >= missileNextFireTime)
            SecondaryFire();
    }

    public void OnPrimaryFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            isPrimaryFireActive = true;

        if (ctx.canceled)
            isPrimaryFireActive = false;
    }

    private void PrimaryFire()
    {
        bulletNextFireTime = Time.time + bulletCooldown;

        SpawnData bullet = new SpawnData();
        bullet.Prefab = bulletPrefab;
        bullet.Position = transform.position + transform.forward;
        bullet.Rotation = transform.rotation;

        // create a spawn event
        gCtrl.spawnEvent.Raise(this.gameObject, bullet);

        // grab the instantiated object using spawn data reference
        Rigidbody bulletRb = bullet.Reference.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;

        // if coming from a red plane
        if (this.CompareTag("red-plane"))
        {
            // set bullet tag
            bullet.Reference.tag = "red-projectile";
        }
        else if (this.CompareTag("blue-plane"))
            bullet.Reference.tag = "blue-projectile";

        DestoryData d = new DestoryData();
        d.Reference = bullet.Reference;
        d.LifeCycle = bulletLifeTime;

        gCtrl.destroyEvent.Raise(this.gameObject, d);
    }

    public void OnSecondaryFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            isSecondaryFireActive = true;

        if (ctx.canceled)
            isSecondaryFireActive = false;
    }

    private void SecondaryFire()
    {
        missileNextFireTime = Time.time + missileCooldown;

        SpawnData missile = new SpawnData();
        missile.Prefab = missilePrefab;
        missile.Position = transform.position + transform.forward;
        missile.Rotation = transform.rotation;

        gCtrl.spawnEvent.Raise(this.gameObject, missile);

        Rigidbody missileRb = missile.Reference.GetComponent<Rigidbody>();
        missileRb.velocity = transform.forward * missileSpeed;

        // if coming from a red plane
        if (this.CompareTag("red-plane"))
            // set missile tag
            missile.Reference.tag = "red-projectile";
        else if (this.CompareTag("blue-plane"))
            missile.Reference.tag = "blue-projectile";

        DestoryData d = new DestoryData(missile.Reference, missileLifeTime);
        gCtrl.destroyEvent.Raise(this.gameObject, d);
    }
}
