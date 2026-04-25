using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;
    public bool isDialogueActive;
    private bool isShowingChoices;
    private DialogueSO currentDialogue;
    private int dialogueIndex;

    private float lastDialogueEndTime;
    private float dialogueCooldown = 0.1f;

    private void Awake()
    {
        isDialogueActive = false;
        isShowingChoices = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        foreach (var button in choiceButtons)
            button.gameObject.SetActive(false);
    }
    public bool CanStartDialogue()
    {
        return Time.unscaledTime - lastDialogueEndTime < dialogueCooldown;
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {

        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        isShowingChoices = false;
        ShowDialogue();
    }

    public void AdvanceDialogue()
    {
        if (isShowingChoices) return;
        Debug.Log("AdvanceDialogue called. Index: " + dialogueIndex + " / Lines: " + currentDialogue.lines.Length);
        if (dialogueIndex < currentDialogue.lines.Length)
            ShowDialogue();
        else
            ShowChoices();
    }

    private void ShowDialogue()
    {
        isShowingChoices = false;
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        GameManager.Instance.DialogueHistoryTracker.RecordNPC(line.speaker);

        portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;

        dialogueText.text = line.text;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        dialogueIndex++;
    }

    private void ShowChoices()
    {
        isShowingChoices = true;
        Debug.Log("ShowChoices called. Button count: " + choiceButtons.Length + " Options: " + currentDialogue.options.Length);
        ClearChoices();
        if (currentDialogue.options.Length > 0)
        {
            for (int i = 0; i < currentDialogue.options.Length; i++)
            {
                if (i >= choiceButtons.Length)
                {
                    Debug.LogWarning("Not enough buttons for options!");
                    break;
                }
                int capturedIndex = i;
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = currentDialogue.options[i].optionText;
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].onClick.AddListener(() => ChooseOption(currentDialogue.options[capturedIndex].nextDialogue));
            }
        }
        else
        {
            choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "End";
            choiceButtons[0].onClick.AddListener(EndDialogue);
            choiceButtons[0].gameObject.SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
    }

    private void ChooseOption(DialogueSO dialogueSO)
    {
        isShowingChoices = false;
        if (dialogueSO == null)
            EndDialogue();
        else
            StartDialogue(dialogueSO);
    }

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        isShowingChoices = false;
        ClearChoices();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        lastDialogueEndTime = Time.unscaledTime;
    }

    private void ClearChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }
}