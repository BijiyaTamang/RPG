using UnityEngine;

public class ToggleSkillTree : MonoBehaviour
{
    public CanvasGroup statsCanvas;
    private bool skillTreeOpen = false;

    void Update()
    {
        if (Input.GetButtonDown("ToggleSkillTree"))
        {
            if (skillTreeOpen)
            {
             
                Time.timeScale = 1; // Resume the game when the skill tree is closed
                statsCanvas.alpha = 0; // Hide the skill tree UI
                statsCanvas.blocksRaycasts = false; // Disable interaction with the skill tree UI
                skillTreeOpen = false; // Update the state to indicate the skill tree is closed
            }
            else
            {
                Time.timeScale = 0; // Pause the game when the skill tree is open
                statsCanvas.alpha = 1; // Show the skill tree UI
                statsCanvas.blocksRaycasts = true; // Enable interaction with the skill tree UI
                skillTreeOpen = true; // Update the state to indicate the skill tree is open
            }
        }
    }
}
