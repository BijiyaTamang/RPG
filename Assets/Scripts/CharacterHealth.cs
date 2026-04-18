using UnityEngine;
using TMPro;
public class CharacterHealth : MonoBehaviour
{
    public int currentHealth; // Current health of the character
   public int maxHealth; // Maximum health of the character
    public TMP_Text health; // Reference to the TextMeshPro UI element that displays health
    public Animator healthAnimation; // Reference to the Animator component for health-related animations
    private void Start()
    {
        health.text = "HP: " + currentHealth + "/" + maxHealth; // Display initial health on the UI
    }
     public void healthChange(int amount) { 
        currentHealth += amount; // Change current health by the specified amount
        healthAnimation.Play("TextMovement"); // Play the health change animation
        health.text = "HP: " + currentHealth + "/" + maxHealth; // Update health display on the UI
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false); // Deactivate the character if health drops to 0 or below
        }
    }
}
