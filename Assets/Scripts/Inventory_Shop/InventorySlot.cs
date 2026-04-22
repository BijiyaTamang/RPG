using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemSO itemSO;
    public int quantity;

    public Image itemImage;
    public TMP_Text quantityText;

    private InventoryManager inventoryManager;

    public void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (quantity > 0)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (itemSO.currentHealth > 0 && StatsManager.Instance.currentHealth >= StatsManager.Instance.maxHealth)
                    return;
                inventoryManager.UseItem(this);
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                inventoryManager.DropLoot(this);
            }
        }
    }

    public void UpdateUI()
    {
        if (itemSO != null)
        {
            itemImage.sprite = itemSO.itemIcon;
            itemImage.gameObject.SetActive(true);
            quantityText.text = quantity.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = "";
        }
    }
        
}
