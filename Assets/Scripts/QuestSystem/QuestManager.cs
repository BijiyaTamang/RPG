using System;
using System.Collections.Generic;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();
    public List<QuestSO> completedQuests = new();
    public List<QuestSO> allQuests; // assign all QuestSOs in Inspector

    private void OnEnable()
    {
        QuestEvents.IsQuestComplete += IsQuestComplete;
    }

    private void OnDisable()
    {
        QuestEvents.IsQuestComplete -= IsQuestComplete;
    }

    public bool IsQuestAccepted(QuestSO questSO)
    {
        return questProgress.ContainsKey(questSO);
    }

    public List<QuestSO> GetActiveQuests()
    {
        return new List<QuestSO>(questProgress.Keys);
    }

    public List<QuestSO> GetCompletedQuests()
    {
        return new List<QuestSO>(completedQuests);
    }

    public void AcceptQuest(QuestSO questSO)
    {
        questProgress[questSO] = new Dictionary<QuestObjective, int>();
        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
    }

    public bool IsQuestComplete(QuestSO questSO)
    {
        if (!questProgress.TryGetValue(questSO, out var progressDict))
            return false;
        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
        foreach (var objective in questSO.objectives)
        {
            if (progressDict[objective] < objective.requiredAmount)
                return false;
        }
        return true;
    }

    public void CompleteQuest(QuestSO questSO)
    {
        questProgress.Remove(questSO);
        completedQuests.Add(questSO);
        foreach (var objective in questSO.objectives)
        {
            if (objective.targetItem != null && objective.requiredAmount > 0)
                InventoryManager.Instance.RemoveItem(objective.targetItem, objective.requiredAmount);
        }
        foreach (var reward in questSO.rewards)
        {
            InventoryManager.Instance.AddItem(reward.itemSO, reward.quantity);
        }
        GameManager.Instance.SaveGame(); // save immediately on quest completion
    }

    public bool GetCompleteQuest(QuestSO questSO)
    {
        return completedQuests.Contains(questSO);
    }

    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective objective)
    {
        if (!questProgress.ContainsKey(questSO))
            return;
        var progressDictionary = questProgress[questSO];
        int newAmount = GetCurrentAmount(questSO, objective);
        if (objective.targetItem != null)
        {
            newAmount = InventoryManager.Instance.GetItemQuantity(objective.targetItem);
        }
        else if (objective.targetLocation != null && GameManager.Instance.LocationHistoryTracker.HasVisited(objective.targetLocation))
        {
            newAmount = objective.requiredAmount;
        }
        else if (objective.targetNPC != null && GameManager.Instance.DialogueHistoryTracker.HasSpokenWith(objective.targetNPC))
        {
            newAmount = objective.requiredAmount;
        }
        progressDictionary[objective] = newAmount;
    }

    public string GetProgressText(QuestSO questSO, QuestObjective objective)
    {
        int currentAmount = GetCurrentAmount(questSO, objective);
        if (currentAmount >= objective.requiredAmount)
            return "Complete";
        else if (objective.targetItem != null)
            return $"{currentAmount}/{objective.requiredAmount}";
        else
            return "In Progress";
    }

    public int GetCurrentAmount(QuestSO questSO, QuestObjective objective)
    {
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
            if (objectiveDictionary.TryGetValue(objective, out int amount))
                return amount;
        return 0;
    }

    public void LoadQuests(List<string> activeQuestNames, List<string> completedQuestNames)
    {
        questProgress.Clear();
        completedQuests.Clear();

        Debug.Log("Loading quests. Active: " + activeQuestNames.Count + " Completed: " + completedQuestNames.Count);

        foreach (var quest in allQuests)
        {
            if (activeQuestNames.Contains(quest.questName))
            {
                questProgress[quest] = new Dictionary<QuestObjective, int>();
                foreach (var objective in quest.objectives)
                {
                    int amount = 0;
                    if (objective.targetItem != null)
                        amount = InventoryManager.Instance.GetItemQuantity(objective.targetItem);
                    else if (objective.targetLocation != null && GameManager.Instance.LocationHistoryTracker.HasVisited(objective.targetLocation))
                        amount = objective.requiredAmount;
                    else if (objective.targetNPC != null && GameManager.Instance.DialogueHistoryTracker.HasSpokenWith(objective.targetNPC))
                        amount = objective.requiredAmount;

                    Debug.Log($"Quest: {quest.questName} | Objective: {objective.description} | Amount: {amount}");
                    questProgress[quest][objective] = amount;
                }
            }
            else if (completedQuestNames.Contains(quest.questName))
            {
                completedQuests.Add(quest);
                Debug.Log("Restored completed quest: " + quest.questName);
            }
        }
    }
}