using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas; // Reference to the CanvasGroup component for controlling the visibility of the stats UI
    private bool statsOpen = false; // Variable to track whether the stats UI is currently open or closed

    private void Start()
    {
        UpdateAllStats();
    }
    private void Update()
    {
        if (Input.GetButtonDown("ToggleStats"))
            if (statsOpen)
            {
                Time.timeScale = 1; // Resume the game by setting the time scale back to 1
                UpdateAllStats(); // Update all stats displays to ensure they show the latest values when the stats UI is closed
                statsCanvas.alpha = 0; // Set the alpha of the CanvasGroup to 0 to make the stats UI invisible
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0; // Pause the game by setting the time scale to 0
                UpdateAllStats(); // Update all stats displays to ensure they show the latest values when the stats UI is opened
                statsCanvas.alpha = 1; // Set the alpha of the CanvasGroup to 1 to make the stats UI visible
                statsOpen = true;
            }
    }
    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage.ToString(); 
        // Update the text of the first stats slot to display the current damage value from the StatsManager
    }
    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.moveSpeed.ToString();
        // Update the text of the first stats slot to display the current damage value from the StatsManager
    }
    public void UpdateAllStats()
    {  
        UpdateDamage(); // Call the method to update the damage stat display
        UpdateSpeed(); // Call the method to update the speed stat display
    }

}
