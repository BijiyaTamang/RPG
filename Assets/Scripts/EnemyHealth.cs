using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health at the start
    }
    public void ChangeHealth(int amount)
    {
        currentHealth += amount; // Change the current health by the specified amount (positive or negative)
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure current health does not exceed maximum health
        }
        else if(currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the enemy GameObject if current health drops to zero or below
        }
    }

}
