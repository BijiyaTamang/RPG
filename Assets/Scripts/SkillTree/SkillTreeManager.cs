using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text skillLevelText;
    public int availableSkillPoints;

    private void OnEnable()
    {
        SkillSlot.OnSkillUpgraded += HandleAbilityUpgrade; // Subscribe to the skill upgrade event
        SkillSlot.OnSkillMaxed += HandleSkillMaxed; // Subscribe to the skill maxed event
        ExpManager.OnLevelUp += UpdateAbilityPoints; // Subscribe to the level up event to update skill points when the player levels up
    }

    private void OnDisable()
    {
        SkillSlot.OnSkillUpgraded -= HandleAbilityUpgrade; // Unsubscribe from the skill upgrade event
        SkillSlot.OnSkillMaxed -= HandleSkillMaxed; // Unsubscribe from the skill maxed event
        ExpManager.OnLevelUp -= UpdateAbilityPoints; // Unsubscribe from the level up event to prevent memory leaks
    }

    private void Start()
    {
        // Initialize skill slots and UI
        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(() => CheckAvailablePoints(slot)); // Add click listener for each skill slot button
        }
        UpdateAbilityPoints(0); // Initialize skill points display
    }
     private void CheckAvailablePoints(SkillSlot slot)
    {
        // Check if the player has enough skill points to upgrade the skill
        if (availableSkillPoints > 0)
        {
            slot.TryUpgradeSkill(); // Attempt to upgrade the skill, which will trigger the OnSkillUpgraded event if successful
        }
        else
        {
            Debug.Log("Not enough skill points!"); // Log a message if the player does not have enough skill points to upgrade the skill
        }
    }
    private void HandleAbilityUpgrade(SkillSlot slot)
    {
        // Handle the skill upgrade logic, such as updating the UI and deducting skill points
        if (availableSkillPoints > 0)
        {
            UpdateAbilityPoints(-1); // Deduct one skill point for the upgrade
        }
        else
        {
            Debug.Log("Not enough skill points!");
        }
    }

    private void HandleSkillMaxed(SkillSlot slot)
    {
        // Handle the logic when a skill is maxed out, such as unlocking dependent skills
        foreach (SkillSlot s in skillSlots)
        {
            // Check if the skill slot is not unlocked and can be unlocked based on its prerequisites
            if (!s.isUnlocked && s.CanUnlockSkill()) // Check if the skill slot is not unlocked and meets the prerequisites for unlocking
                s.Unlock(); // Unlock the skill slot if it meets the prerequisites
        }
    }

    public void UpdateAbilityPoints(int amount)
    {
        // Update the available skill points and refresh the UI display
        availableSkillPoints += amount;
        skillLevelText.text = "Skill Points: " + availableSkillPoints.ToString(); // Update the skill points display in the UI
    }
}