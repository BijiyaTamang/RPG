using UnityEngine;

public class CharacterChangeEquipment : MonoBehaviour
{
    public CharacterCombat combat;
    public CharacterBow bow;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("ChangeEquipment"))
        {
            bow.enabled = !bow.enabled; // Toggle the enabled state of the bow component
        }
    }
}
