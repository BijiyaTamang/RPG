using UnityEngine;
using System.Collections.Generic;

public class QuestLogDebug : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private QuestLogUI questLogUI;

    private void Start()
    {
        Debug.Log("=== QUEST LOG DEBUG ON START ===");
        Debug.Log($"QuestManager assigned: {questManager != null}");
        Debug.Log($"QuestLogUI assigned: {questLogUI != null}");

        if (questManager != null)
        {
            List<QuestSO> active = questManager.GetActiveQuests();
            Debug.Log($"Active quests on start: {active.Count}");
            foreach (var q in active)
                Debug.Log($"  - {q.questName}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("=== F1 QUEST STATE DUMP ===");
            List<QuestSO> active = questManager.GetActiveQuests();
            Debug.Log($"Active quests: {active.Count}");
            foreach (var q in active)
            {
                Debug.Log($"  Quest: {q.questName}");
                Debug.Log($"    IsAccepted: {questManager.IsQuestAccepted(q)}");
                Debug.Log($"    IsComplete(manager): {questManager.IsQuestComplete(q)}");
                Debug.Log($"    GetCompleteQuest: {questManager.GetCompleteQuest(q)}");
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("=== F2 SIMULATE OPEN QUEST LOG ===");
            List<QuestSO> active = questManager.GetActiveQuests();
            Debug.Log($"Active quests: {active.Count}");
            foreach (var q in active)
            {
                Debug.Log($"  Quest: {q.questName}");
                Debug.Log($"    IsComplete: {questManager.IsQuestComplete(q)}");
                Debug.Log($"    GetCompleteQuest: {questManager.GetCompleteQuest(q)}");
            }
            questLogUI.OpenQuestLog();
        }
    }

    private void OnEnable()
    {
        QuestEvents.OnQuestOfferRequested += OnOffer;
        QuestEvents.OnQuestTurnInRequested += OnTurnIn;
        QuestEvents.OnQuestAccepted += OnAccepted;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestOfferRequested -= OnOffer;
        QuestEvents.OnQuestTurnInRequested -= OnTurnIn;
        QuestEvents.OnQuestAccepted -= OnAccepted;
    }

    private void OnOffer(QuestSO q)
    {
        Debug.Log($"[EVENT: QuestOfferRequested] Quest: {q?.questName ?? "NULL"}");
        if (q != null && questManager != null)
        {
            Debug.Log($"  IsAccepted: {questManager.IsQuestAccepted(q)}");
            Debug.Log($"  IsComplete(manager): {questManager.IsQuestComplete(q)}");
            Debug.Log($"  GetCompleteQuest: {questManager.GetCompleteQuest(q)}");
        }
    }

    private void OnTurnIn(QuestSO q)
    {
        Debug.Log($"[EVENT: QuestTurnInRequested] Quest: {q?.questName ?? "NULL"}");
        if (q != null && questManager != null)
        {
            Debug.Log($"  IsAccepted: {questManager.IsQuestAccepted(q)}");
            Debug.Log($"  IsComplete(manager): {questManager.IsQuestComplete(q)}");
            Debug.Log($"  GetCompleteQuest: {questManager.GetCompleteQuest(q)}");
        }
    }

    private void OnAccepted(QuestSO q)
    {
        Debug.Log($"[EVENT: QuestAccepted] Quest: {q?.questName ?? "NULL"}");
        if (q != null && questManager != null)
        {
            Debug.Log($"  IsAccepted after: {questManager.IsQuestAccepted(q)}");
        }
    }
}