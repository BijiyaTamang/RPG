using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer sr;
    public Animator anim;
    public bool canBePickedUp = true;
    public int quantity;
    public float respawnTime = 10f; // Time in seconds before item respawns
    public static event Action<ItemSO, int> OnLootPickUp;

    private void OnValidate()
    {
        if (itemSO != null)
            return;
    }

    public void Initialize(ItemSO itemSO, int quantity)
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
        canBePickedUp = false;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        sr.sprite = itemSO.itemIcon;
        this.name = itemSO.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBePickedUp == true)
        {
            anim.Play("LootPickUp");
            OnLootPickUp?.Invoke(itemSO, quantity);
            StartCoroutine(RespawnCoroutine()); // Start respawn instead of Destroy
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePickedUp = true;
        }
    }

    IEnumerator RespawnCoroutine()
    {

        canBePickedUp = false;
        sr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(respawnTime);
        sr.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        canBePickedUp = true;
        anim.Play("Idle"); // Stop the pickup animation and reset to idle
    }
}