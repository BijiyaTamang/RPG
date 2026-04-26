using TMPro;
using UnityEngine;
public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;
    private bool statsOpen = false;
    private void Start()
    {
        UpdateAllStats();
    }
    private void Update()
    {
        if (Input.GetButtonDown("ToggleStats"))
            if (statsOpen)
            {
                Time.timeScale = 1;
                UpdateAllStats();
                statsCanvas.alpha = 0;
                statsCanvas.blocksRaycasts = false;
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                UpdateAllStats();
                statsCanvas.alpha = 1;
                statsCanvas.blocksRaycasts = true;
                statsOpen = true;
            }
    }
    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Knight Damage: " + StatsManager.Instance.damage.ToString();
    }
    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.moveSpeed.ToString();
    }
    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Archer Damage: " + StatsManager.Instance.arrowDamage.ToString();
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Bow Speed: " + StatsManager.Instance.arrowSpeed.ToString();
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = "Bow Stun: " + StatsManager.Instance.arrowStunTime.ToString();
    }
}