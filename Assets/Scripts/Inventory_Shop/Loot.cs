using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer sr;
    public Animator anim;

    public int quantity;
    public static event Action<ItemSO, int> OnLootPickUp;

    private void OnValidate()
    {
        if (itemSO != null)
            return;
        {
            sr.sprite = itemSO.itemIcon;
            this.name = itemSO.itemName;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.Play("LootPickUp");
            OnLootPickUp?.Invoke(itemSO, quantity);
            Destroy(gameObject, 0.5f);
        }
    }
}

