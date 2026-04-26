using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName; // Name of the item
    [TextArea] public string itemDescription; // Description of the item
    public Sprite itemIcon; // Icon representing the item in the UI
    public bool isGold;
    public bool isExp;
    public int stackLimit = 10;
    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public int moveSpeed;
    public int damage;
    [Header("Temporary Effects")]
    public float duration;
    [Header("Arrow Stats")]
    public int arrowDamage; // Damage dealt by the arrow
    public float arrowSpeed; // Speed at which the arrow travels
    [Header("Gold")]
    public int goldAmount; // Amount of gold given when item is picked up
}