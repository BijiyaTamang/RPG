using UnityEngine;
public class CharacterChangeEquipment : MonoBehaviour
{
    public CharacterCombat combat;
    public CharacterBow bow;

    void Update()
    {
        if (Input.GetButtonDown("ChangeEquipment"))
        {
            if (!SkillManager.bowUnlocked)
            {
                Debug.Log("Archer not unlocked yet!");
                return;
            }
            bow.enabled = !bow.enabled;
        }
    }
}