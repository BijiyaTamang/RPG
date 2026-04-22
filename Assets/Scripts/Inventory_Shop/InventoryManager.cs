using UnityEngine;
using TMPro;
using System.Collections;
public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] itemSlots;
    public int gold;
    public TMP_Text goldText;

    private void Start()
    {
        foreach(var slot in itemSlots)
        {
            slot.UpdateUI();
        }
    }
    private void OnEnable()
    {
        Loot.OnLootPickUp += AddItem;
    }
    private void OnDisable()
    {
        Loot.OnLootPickUp -= AddItem;
    }
    public void AddItem(ItemSO itemSO, int quantity)
    {
        if(itemSO.isGold)
        {
            gold += quantity;
            goldText.text = gold.ToString();
            return;
        }
        else
        {
            foreach(var slot in itemSlots)
            {
                if(slot.itemSO == null)
                {
                    slot.itemSO = itemSO;
                    slot.quantity = quantity;
                    slot.UpdateUI();
                    return;
                }
            }
        }
    }
}
