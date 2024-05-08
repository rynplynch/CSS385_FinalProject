using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public string healthBarTag = "player-health-bar";

    private HealthBar healthBar;
    private bool foundObject = false;
    private GameObject healthBarObject;

    private GameLogic gCtrl;

    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!foundObject)
        {
            healthBarObject = GameObject.FindWithTag(healthBarTag);
            foreach (Transform child in healthBarObject.transform)
            {
                child.gameObject.SetActive(true);
            }
            healthBar = healthBarObject.GetComponentInChildren<HealthBar>();
            healthBar.SetMaxHealth(maxHealth);
            foundObject = !foundObject;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            DestoryData d = new DestoryData(this.gameObject, 0);
            gCtrl.destroyEvent.Raise(this.gameObject, d);
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
}
