using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public CharacterCombat combat; // Reference to the CharacterCombat component to apply stat changes when skills are upgraded
    private void Start()
    {
        combat.enabled = false; // ensure it's disabled regardless of Inspector setting
    }
    private void OnEnable()
    {
        SkillSlot.OnSkillUpgraded += HandleSkillUpgraded; // Subscribe to the OnSkillSlotClicked event when the object is enabled
    }
    private void OnDisable()
    {
        SkillSlot.OnSkillUpgraded -= HandleSkillUpgraded; // Unsubscribe from the OnSkillSlotClicked event when the object is disabled
    }
    
    private void HandleSkillUpgraded(SkillSlot slot)
    {
        string skillName = slot.skillSO.skillName;

        switch (skillName)
        {
            case "Basic":
                combat.enabled = true;
                break;
            case "Health Boost 2":
                StatsManager.Instance.UpdateMaxHealth(1);
                break;
            default: 
                Debug.LogWarning("Unknown Skill: " + skillName);
                break;
        }
    }
}
