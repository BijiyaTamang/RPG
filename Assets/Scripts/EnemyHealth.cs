using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int expReward = 3;
    public delegate void EnemyDeath(int exp); // Define a delegate type for the monster death event, which takes an integer parameter for experience reward
   public static event EnemyDeath OnEnemyDeath; // Declare a static event of the MonsterDeath delegate type
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
            OnEnemyDeath(expReward); // Trigger the OnEnemyDeath event, passing the experience reward as an argument
            Destroy(gameObject); // Destroy the enemy GameObject if current health drops to zero or below
        }

    }

}
