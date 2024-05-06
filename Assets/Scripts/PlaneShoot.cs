using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private float bulletNextFireTime;
    private float missileNextFireTime;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= bulletNextFireTime) // Left mouse button
        //if (Input.GetMouseButton(0))
        {
            ShootBullet();
            bulletNextFireTime = Time.time + bulletCooldown;
        }

        if (Input.GetMouseButtonDown(1) && Time.time >= missileNextFireTime) //Right mouse button
        {
            ShootMissile();
            missileNextFireTime = Time.time + missileCooldown;
        }
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);
        bullet.tag = gameObject.tag; // pass along gameobject tag to bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * bulletSpeed;
        Destroy(bullet, bulletLifeTime); // destroy self after certain amount of time as passed
    }

    void ShootMissile()
    {
        GameObject missile = Instantiate(missilePrefab, transform.position + transform.forward, transform.rotation);
        missile.tag = gameObject.tag; // pass along gameobject tag to bullet
        Rigidbody missileRb = missile.GetComponent<Rigidbody>();
        missileRb.velocity = transform.forward * missileSpeed;
        Destroy(missile, missileLifeTime); // destroy self after certain amount of time as passed
    }
}
