using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestSO", menuName = "Scriptable Objects/QuestSO")]
public class QuestSO : ScriptableObject
{
    public string questName;
    [TextArea] public string questDescription;
    public int questLevel;

    public List<QuestObjective> objectives;
    public List<QuestReward> rewards;

}

[System.Serializable]
public class QuestObjective
{
    public string description;

    [SerializeField] private Object target;

    public ItemSO targetItem => target as ItemSO;
    public ActorSO targetNPC => target as ActorSO;
    public LocationSO targetLocation => target as LocationSO;

    public int requiredAmount;

}

[System.Serializable]
public class QuestReward
{
    public ItemSO itemSO;
    public int quantity;
}
