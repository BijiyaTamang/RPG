using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ExpManager : MonoBehaviour
{
    public int level;
    public int currentExp;
    public int expToLevel = 10;
    public float expGrowthMultiplier = 1.2f;
    public Slider expSlider; // Reference to the UI Slider component for displaying experience progress
    public TMP_Text currentLevelText; // Reference to the UI Text component for displaying the current level

    public static event Action<int> OnLevelUp; // Event to notify when the character levels up, passing the new level as an argument
    private void Start()
    {
        UpdateExpUI(); // Call the UpdateExpUI method to initialize the experience UI elements
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) // Check if the "V" key is pressed
        {
            GainExperience(2); // Call the GainExperience method with an amount of 5 to simulate gaining experience
        }
    }
    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += GainExperience; // Subscribe the GainExperience method to the OnEnemyDeath event when the script is enabled
    }
    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= GainExperience; // Unsubscribe the GainExperience method from the OnEnemyDeath event when the script is disabled to prevent memory leaks
    }
    public void GainExperience(int amount)
    {
        currentExp += amount; // Increase the current experience by the specified amount
        if (currentExp >= expToLevel) // Check if the current experience has reached or exceeded the threshold for leveling up
        {
            LevelUp(); // Call the LevelUp method to handle the leveling up process
        }
        UpdateExpUI(); // Call the UpdateExpUI method to update the experience UI elements after gaining experience
    }   
    private void LevelUp()
    {
        level++; // Increase the character's level by 1
        currentExp -= expToLevel; // Subtract the experience required for leveling up from the current experience
        expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultiplier); // Increase the experience required for the next level by 5
        OnLevelUp?.Invoke(1); // Invoke the OnLevelUp event to notify subscribers that the character has leveled up, passing the new level as an argument
    }
    public void UpdateExpUI()
    {
        expSlider.maxValue = expToLevel; // Set the maximum value of the experience slider to the experience required for leveling up
        expSlider.value = currentExp; // Set the current value of the experience slider to the current experience
        currentLevelText.text = "Level: " + level; // Update the current level text to display the current level
    }

}
