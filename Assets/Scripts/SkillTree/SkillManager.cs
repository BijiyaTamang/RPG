using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class SkillManager : MonoBehaviour
{
    public CharacterCombat combat;
    public CharacterBow bow;
    public static bool bowUnlocked = false;
    private void Start()
    {
        combat.enabled = false;
        bow.enabled = false;
    }
    private void OnEnable()
    {
        SkillSlot.OnSkillUpgraded += HandleSkillUpgraded;

    }
    private void OnDisable()
    {
        SkillSlot.OnSkillUpgraded -= HandleSkillUpgraded;
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
                StatsManager.Instance.UpdateMaxHealth(2);
                break;
            case "Health Boost 3":
                StatsManager.Instance.UpdateMaxHealth(3);
                break;
            case "Health Boost 4":
                StatsManager.Instance.UpdateMaxHealth(3);
                break;

            case "Damage1":
            case "Damage2":
            case "Damage3":
            case "Damage4":
                StatsManager.Instance.UpdateDamage(1);
                break;

            case "Speed1":
            case "Speed2":
            case "Speed3":
            case "Speed4":
                StatsManager.Instance.UpdateSpeed(0.1f);
                break;

            default:
                Debug.LogWarning("Unknown Skill: " + skillName);
                break;

            // Archer skills
            case "ArcherDamage1":
                bowUnlocked = true; // just set the flag
                break;
            case "ArcherDamage2":
                StatsManager.Instance.UpdateArrowDamage(1);
                break;
            case "ArcherDamage3":
                StatsManager.Instance.UpdateArrowDamage(1);
                break;
            case "ArcherDamage4":
                StatsManager.Instance.UpdateArrowDamage(2);
                break;

            case "BowSpeed1":
            case "BowSpeed2":
            case "BowSpeed3":
            case "BowSpeed4":
                StatsManager.Instance.UpdateArrowSpeed(0.1f);
                break;

            case "BowStun1":
            case "BowStun2":
            case "BowStun3":
            case "BowStun4":
                StatsManager.Instance.UpdateArrowStun(0.1f);
                break;
        }
    }
}