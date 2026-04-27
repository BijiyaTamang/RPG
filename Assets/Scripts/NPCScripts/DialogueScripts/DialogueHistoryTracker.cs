using System.Collections.Generic;
using UnityEngine;
public class DialogueHistoryTracker : MonoBehaviour
{
    private HashSet<ActorSO> spokenNPCs = new HashSet<ActorSO>();
    public List<ActorSO> allActors; // assign all ActorSOs in Inspector

    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);
        Debug.Log("Just spoke to " + actorSO.actorName);
    }

    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);
    }

    public List<string> GetSpokenNPCNames()
    {
        var names = new List<string>();
        foreach (var npc in spokenNPCs)
            names.Add(npc.actorName);
        return names;
    }

    public void LoadSpokenNPCs(List<string> names)
    {
        spokenNPCs.Clear();
        foreach (var actor in allActors)
            if (names.Contains(actor.actorName))
                spokenNPCs.Add(actor);
    }
}