using UnityEngine;
using System.Collections.Generic;

public class NPCSheep : MonoBehaviour
{
    public enum NPCState { Default, Idle, Patrol, Wander }
    public NPCState currentState = NPCState.Patrol;
    private NPCState defaultState;

    public NPC_Patrol patrol;
    public NPC_Wander wander;



    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {
        currentState = newState;

        patrol.enabled = newState == NPCState.Patrol;
        wander.enabled = newState == NPCState.Wander;
    }
}
