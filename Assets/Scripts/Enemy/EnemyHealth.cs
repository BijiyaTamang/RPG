using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int expReward = 3;
    public delegate void EnemyDeath(int exp);
    public static event EnemyDeath OnEnemyDeath;
    public int currentHealth;
    public int maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnEnemyDeath?.Invoke(expReward);
            GetComponent<EnemyRespawn>().Die(); // Call respawn instead of Destroy
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}