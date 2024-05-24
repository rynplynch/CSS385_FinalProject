using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBotAi : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float turnSpeed = 5f;
    private float xRange = 900f;
    private float zRange = 900f;
    private float yPosition = 75f;
    public float detectionRange = 100f; 

    private Vector3 targetPosition;

    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    private float bulletSpeed = 5.0f;
    private float missileSpeed = 5.0f;
    private float bulletCooldown = 0.4f;
    private float missileCooldown = 10.0f;
    private float bulletLifeTime = 2.0f; // Amount of time passes before destroy self
    private float missileLifeTime = 5.0f; // Amount of time passes before destroy self

    // used to invoke spawn event
    private Rigidbody rb;
    private GameLogic gCtrl;
    private float bulletNextFireTime;
    private float missileNextFireTime;

    private GameObject currentTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
        }
        gCtrl = GameLogic.Instance;
        SetNewRandomTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            currentTarget = DetectTarget();
            if (currentTarget == null)
            {
                MoveTowardsTarget();
            }
        }
        else
        {
            LockOnToTarget();
            AttackTarget();
        }
    }

    void SetNewRandomTargetPosition()
    {
        float randomX = Random.Range(-xRange / 2, xRange / 2);
        float randomZ = Random.Range(-zRange / 2, zRange / 2);
        targetPosition = new Vector3(randomX, yPosition, randomZ);
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > 1f)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            SetNewRandomTargetPosition();
        }
    }

    void LockOnToTarget()
    {
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        if (Vector3.Distance(transform.position, currentTarget.transform.position) > 1f)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void AttackTarget()
    {
        if (Time.time >= missileNextFireTime)
        {
            ShootMissile();
            missileNextFireTime = Time.time + missileCooldown;
        }
        else if (Time.time >= bulletNextFireTime)
        {
            ShootBullet();
            bulletNextFireTime = Time.time + bulletCooldown;
        }

        // PlaneShoot shootingScript = GetComponent<PlaneShoot>();
        // if (shootingScript != null)
        // {
        //     if (Time.time >= missileNextFireTime)
        //     {
        //         shootingScript.ShootMissile();
        //         missileNextFireTime = Time.time + missileCooldown;
        //     }
        //     else
        //     {
        //         shootingScript.ShootBullet();
        //     }
        // }
    }

    GameObject DetectTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in colliders)
        {
            GameObject o = collider.gameObject;
            if (o && (CheckTag.IsBoat(o) || CheckTag.IsPlane(o)))
            {
                // Check if the target is of matching color
                if (!CheckTag.MatchingColor(this.gameObject.tag, collider.tag))
                {
                return o;
                }
            }
        }
        return null;
    }

    void ShootBullet()
    {
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
        if (this.CompareTag("red-plane-bot"))
        {
            // set bullet tag
            bullet.Reference.tag = "red-projectile";
        }
        else if (this.CompareTag("blue-plane-bot"))
            bullet.Reference.tag = "blue-projectile";

        DestoryData d = new DestoryData();
        d.Reference = bullet.Reference;
        d.LifeCycle = bulletLifeTime;

        gCtrl.destroyEvent.Raise(this.gameObject, d);
    }

    void ShootMissile()
    {
        SpawnData missile = new SpawnData();
        missile.Prefab = missilePrefab;
        missile.Position = transform.position + transform.forward;
        missile.Rotation = transform.rotation;

        gCtrl.spawnEvent.Raise(this.gameObject, missile);

        Rigidbody missileRb = missile.Reference.GetComponent<Rigidbody>();
        missileRb.velocity = transform.forward * missileSpeed;

        // if coming from a red plane
        if (this.CompareTag("red-plane-bot"))
            // set missile tag
            missile.Reference.tag = "red-projectile";
        else if (this.CompareTag("blue-plane-bot"))
            missile.Reference.tag = "blue-projectile";

        DestoryData d = new DestoryData(missile.Reference, missileLifeTime);
        gCtrl.destroyEvent.Raise(this.gameObject, d);
    }
}