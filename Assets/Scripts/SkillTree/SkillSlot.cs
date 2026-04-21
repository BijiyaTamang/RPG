using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;
public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots;
    public SkillSO skillSO;
    public Image skillIcon;
    public Button skillButton;
    public TMP_Text skillLevelText;
    public int currentLevel;
    public bool isUnlocked;
    // Events to notify when a skill is upgraded or maxed out
    public static event Action<SkillSlot> OnSkillUpgraded;
    // Event to notify when a skill is maxed out
    public static event Action<SkillSlot> OnSkillMaxed;


    public void OnValidate()
    {
        // This method is called in the editor when the script is loaded or a value is changed in the inspector
        if (skillSO != null && skillLevelText != null)
        {
            UpdateUI(); // Update the UI to reflect the current state of the skill slot whenever a change is made in the inspector
        }
    }

    public void TryUpgradeSkill()
    {
        // Check if the skill can be upgraded (is unlocked and not at max level)
        if (isUnlocked && currentLevel < skillSO.maxLevel)
        {
            // Upgrade the skill by increasing the current level
            currentLevel++;
            OnSkillUpgraded?.Invoke(this); // Invoke the OnSkillUpgraded event to notify subscribers that the skill has been upgraded
            // Check if the skill has reached its maximum level after the upgrade
            if (currentLevel >= skillSO.maxLevel)
            {
                // Invoke the OnSkillMaxed event to notify subscribers that the skill has reached its maximum level
                OnSkillMaxed?.Invoke(this);
            }
            UpdateUI(); // Update the UI to reflect the new level of the skill after the upgrade
        }
    }

    public bool CanUnlockSkill()
    {
        // Check if all prerequisite skill slots are unlocked and at their maximum level
        foreach (SkillSlot slot in prerequisiteSkillSlots)
        {
            // If any prerequisite skill slot is not unlocked or not at max level, return false
            if (!slot.isUnlocked || slot.currentLevel < slot.skillSO.maxLevel)
            {
                return false; // The skill cannot be unlocked because at least one prerequisite skill slot is not unlocked or not at max level
            }
        }
        // If all prerequisite skill slots are unlocked and at max level, return true to indicate that the skill can be unlocked
        return true;
    }
    public void Unlock()
    {
        // Unlock the skill slot by setting isUnlocked to true and updating the UI to reflect the unlocked state
        isUnlocked = true;
        UpdateUI(); // Update the UI to reflect the unlocked state of the skill slot after unlocking it
    }
    private void UpdateUI()
    {
        // Update the skill icon, button interactability, and level text based on the current state of the skill slot
        skillIcon.sprite = skillSO.skillIcon;
        // If the skill slot is unlocked, enable the button and display the current level; otherwise, disable the button and show "Locked"
        if (isUnlocked)
        {
            // If the skill slot is unlocked, enable the button, display the current level and max level, and set the icon color to white
            skillButton.interactable = true;
            // Display the current level and max level in the skill level text (e.g., "1/5" for level 1 out of 5)
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();
            // Set the skill icon color to white to indicate that the skill is unlocked and can be upgraded
            skillIcon.color = Color.white;
        }
        else
        {
            // If the skill slot is locked, disable the button, show "Locked" in the skill level text, and set the icon color to gray
            skillButton.interactable = false;
            // Display "Locked" in the skill level text to indicate that the skill is locked and cannot be upgraded
            skillLevelText.text = "Locked";
            // Set the skill icon color to gray to indicate that the skill is locked and cannot be upgraded
            skillIcon.color = Color.gray;
        }
    }
}
