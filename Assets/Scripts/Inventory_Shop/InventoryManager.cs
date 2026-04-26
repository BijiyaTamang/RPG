using UnityEngine;
using TMPro;
using System;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventorySlot[] itemSlots;
    public UseItem useItem;
    public int gold;
    public TMP_Text goldText;
    public GameObject lootPrefab;
    public Transform player;

    public static event Action<int> OnExperienceGained;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        foreach(var slot in itemSlots)
        {
            slot.UpdateUI();
        }
        goldText.text = gold.ToString();
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
        if (itemSO.isGold)
        {
            gold += itemSO.goldAmount > 0 ? itemSO.goldAmount : quantity;
            goldText.text = gold.ToString();
            return;
        }
        if (itemSO.isExp)
        {
            OnExperienceGained?.Invoke(quantity);
            return;
        }
        foreach (var slot in itemSlots)
        {
            if(slot.itemSO == itemSO && slot.quantity < itemSO.stackLimit)
            {
                int availableSpace = itemSO.stackLimit - slot.quantity;
                int amountToAdd = Mathf.Min(availableSpace, quantity);

                slot.quantity += amountToAdd;
                quantity -= amountToAdd;
                slot.UpdateUI();    

                if(quantity <= 0)
                    return;
            }
        }
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null)
            {
                int amountToAdd = Mathf.Min(itemSO.stackLimit, quantity);
                slot.itemSO = itemSO;
                slot.quantity = quantity;
                slot.UpdateUI();
                return;
            }
        }
        if (quantity > 0)
        {
            DropLoot(itemSO, quantity);
        }
    }

    public void RemoveItem(ItemSO itemSO, int quantity)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i];

            if (slot.itemSO != itemSO)
                continue;
            if(slot.quantity > quantity)
            {
                slot.quantity -= quantity;   
                slot.UpdateUI();
                quantity = 0;

            }
            else
            {
                quantity -= slot.quantity;
                slot.itemSO = null;
                slot.quantity = 0;
                slot.UpdateUI();
            }
        }

    }
    public void DropItem(InventorySlot slot)
    {
        if (slot.itemSO != null && slot.quantity > 0)
        {
            DropLoot(slot.itemSO, 1);
            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.itemSO = null;
            }
            slot.UpdateUI();
        }
    }

    private void DropLoot(ItemSO itemSO, int quantity)
    {
        Loot loot = Instantiate(lootPrefab, player.position, Quaternion.identity).GetComponent<Loot>();
        loot.Initialize(itemSO, quantity);
    }

    public void UseItem(InventorySlot slot)
    {
        if(slot.itemSO != null && slot.quantity >= 0)
        {
            useItem.ApplyItemEffects(slot.itemSO);

            slot.quantity--;
            if(slot.quantity <= 0)
            {
                slot.itemSO = null;
            }
            slot.UpdateUI();
        }
    }

    public bool HasItem(ItemSO itemSO)
    {
        foreach(var slot in itemSlots)
        {
            if(slot.itemSO == itemSO && slot.quantity > 0) 
                return true;
        }
        return false;
    }

    public int GetItemQuantity(ItemSO itemSO)
    {
        int total = 0;

        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO)
                total += slot.quantity;
        }

        return total;
    }
}
