using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName; // Name of the item
    [TextArea] public string itemDescription; // Description of the item
    public Sprite itemIcon; // Icon representing the item in the UI

    public bool isGold;
    public int currenHealth;
    public int maxHealth;
    public int moveSpeed;
    public int damage;

} 