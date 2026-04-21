using UnityEngine;
using TMPro;
public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance; // Static instance for global access to the StatsManager
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
    }


}
