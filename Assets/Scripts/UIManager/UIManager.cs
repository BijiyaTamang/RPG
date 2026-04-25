using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuBar;
    private bool isMenuActive;

    [SerializeField] private CanvasGroup statsMenu;
    [SerializeField] private CanvasGroup skillsMenu;
    [SerializeField] private CanvasGroup questMenu;


    [SerializeField] private QuestLogUI questLogUI;

    public void ToggleMenu(CanvasGroup target)
    {
        SetMenuState(statsMenu, false);
        SetMenuState(questMenu, false);
        SetMenuState(skillsMenu, false);
        SetMenuState(target, true);

        // Refresh quest log whenever it's opened from the mini menu
        if (target == questMenu)
        {
            questLogUI.OpenQuestLog();
        }
    }
    public void ToggleMainMenu()
    {
        isMenuActive = !isMenuActive;
        SetMenuState(menuBar, isMenuActive);

        SetMenuState(statsMenu, false);
        SetMenuState(questMenu, false);
        SetMenuState(skillsMenu, false);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void SetMenuState(CanvasGroup group, bool isActive)
    {
        group.alpha = isActive ? 1 : 0;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;


    }
}
