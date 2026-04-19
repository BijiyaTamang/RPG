using UnityEngine;
using TMPro;
public class CharacterHealth : MonoBehaviour
{
    public TMP_Text health; // Reference to the TextMeshPro UI element that displays health
    public Animator healthAnimation; // Reference to the Animator component for health-related animations
    private void Start()
    {
        health.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth; // Display initial health on the UI
    }
     public void healthChange(int amount) { 
        StatsManager.Instance.currentHealth += amount; // Change current health by the specified amount
        healthAnimation.Play("TextMovement"); // Play the health change animation
        health.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth; // Update health display on the UI
        if (StatsManager.Instance.currentHealth <= 0)
        {
            gameObject.SetActive(false); // Deactivate the character if health drops to 0 or below
        }
    }
}
