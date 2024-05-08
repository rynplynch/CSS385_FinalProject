using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth; // Maximum health
    private int currentHealth; // Current health
    private TextMesh healthText; // TextMesh component to display health
    private GameObject healthTextObject; // Reference to the GameObject containing health text

    private GameLogic gCtrl;




    // Start is called before the first frame update
    void Start()
    {
        gCtrl = GameLogic.Instance;

        currentHealth = maxHealth; // Set current health to max health at the start

        // Create an empty GameObject for health text
        healthTextObject = new GameObject("HealthText");
        healthTextObject.transform.SetParent(transform); // Set the main GameObject as parent
        healthTextObject.transform.localPosition = Vector3.up * 2f; // Position of text

        // Add TextMesh component to the health text GameObject
        healthText = healthTextObject.AddComponent<TextMesh>();

        // Configure the health text properties
        healthText.characterSize = 2f; // Character
        healthText.fontSize = 24; // Font size
        healthText.color = Color.blue; // Color
        healthText.alignment = TextAlignment.Center;
        healthText.anchor = TextAnchor.MiddleCenter;

        UpdateHealthText(); // Update the displayed health
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= maxHealth && currentHealth > maxHealth / 2)
        {
            healthText.color = Color.green;
        }
        else if (currentHealth <= maxHealth / 2 && currentHealth > maxHealth / 4)
        {
            healthText.color = Color.yellow;
        }
        else if (currentHealth <= maxHealth / 20 && currentHealth > 0)
        {
            healthText.color = Color.red;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            // destroy data telling the destroy event to remove this gameobject
            DestoryData d = new DestoryData(this.gameObject, 0);

            // also pass in the caller into the spawn event
            gCtrl.destroyEvent.Raise(this.gameObject, d);
        }
        UpdateHealthText(); // Update the displayed health after taking damage
    }

    void UpdateHealthText()
    {
        // Update the health text to display the current health value
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
}
