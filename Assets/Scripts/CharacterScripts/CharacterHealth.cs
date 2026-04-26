using UnityEngine;
using TMPro;
using System.Collections;
public class CharacterHealth : MonoBehaviour
{
    public TMP_Text health;
    public Animator healthAnimation;
    private void Start()
    {
        health.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
    }
    public void healthChange(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        healthAnimation.Play("TextMovement");
        health.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
        if (StatsManager.Instance.currentHealth <= 0)
        {
            StartCoroutine(RespawnRoutine());
        }
    }
    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        PlayerRespawn.Instance.Respawn();
        health.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth; // refresh UI after respawn
    }
}