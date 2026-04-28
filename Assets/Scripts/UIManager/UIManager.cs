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
    [SerializeField] private CanvasGroup exitCanvas;
    [SerializeField] private QuestLogUI questLogUI;

    public void ToggleMenu(CanvasGroup target)
    {
        SetMenuState(statsMenu, false);
        SetMenuState(questMenu, false);
        SetMenuState(skillsMenu, false);
        SetMenuState(exitCanvas, false);
        SetMenuState(target, true);

        if (target == questMenu)
            questLogUI.OpenQuestLog();
    }

    public void ToggleMainMenu()
    {
        isMenuActive = !isMenuActive;
        SetMenuState(menuBar, isMenuActive);
        SetMenuState(statsMenu, false);
        SetMenuState(questMenu, false);
        SetMenuState(skillsMenu, false);
        SetMenuState(exitCanvas, false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ShowExitCanvas()
    {
        SetMenuState(statsMenu, false);
        SetMenuState(questMenu, false);
        SetMenuState(skillsMenu, false);
        SetMenuState(exitCanvas, true);
    }

    public void HideExitCanvas()
    {
        SetMenuState(exitCanvas, false);
    }

    public void ConfirmExit()
    {
        GameManager.Instance.QuitGame();
    }


    private void SetMenuState(CanvasGroup group, bool isActive)
    {
        group.alpha = isActive ? 1 : 0;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;
    }
}