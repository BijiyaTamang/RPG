using UnityEngine;
using TMPro;
public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance; // Static instance for global access to the StatsManager
    public StatsUI statsUI; // Reference to the StatsUI component for updating the UI
    public TMP_Text healthText; // Reference to the UI Text component for displaying health

    [Header("Combat Stats")]
    public int damage;
    public float weaponRange;
    public float knockbackForce;
    public float knockbackDuration;
    public float stunTime;


    [Header("Movement Stats")]
    public float moveSpeed;
    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the static instance to this object
        }
        else
        {
            Destroy(gameObject); // Destroy this object if another instance already exists
        }
    }
    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
        healthText.text = "HP: " + currentHealth + "/ " + maxHealth; // Update the health text to display the current health and maximum health
        statsUI.UpdateAllStats(); // Update the health UI through the StatsUI component
    }
    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth; // Ensure current health does not exceed maximum health
        healthText.text = "HP: " + currentHealth + "/ " + maxHealth; // Update the health text to display the current health and maximum health
    }
    public void UpdateSpeed(float amount)
    {
        moveSpeed += amount;
        statsUI.UpdateAllStats(); // Update the speed UI through the StatsUI component
    }
    public void UpdateDamage(int amount)
    {
        damage += amount;
        statsUI.UpdateAllStats();
    }
    [Header("Arrow Stats")]
    public int arrowDamage;
    public float arrowSpeed;
    public float arrowStunTime;

    public void UpdateArrowDamage(int amount)
    {
        arrowDamage += amount;
        statsUI.UpdateAllStats();
    }

    public void UpdateArrowSpeed(float amount)
    {
        arrowSpeed += amount;
        statsUI.UpdateAllStats();
    }

    public void UpdateArrowStun(float amount)
    {
        arrowStunTime += amount;
        statsUI.UpdateAllStats();
    }
}
