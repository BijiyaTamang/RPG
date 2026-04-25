using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;
public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactAnim;

    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        QuestEvents.OnQuestAccepted += OnQuestAccepted_RemovedOfferings;
    }

    private void OnDestroy()
    {
        QuestEvents.OnQuestAccepted -= OnQuestAccepted_RemovedOfferings;

    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        anim.Play("Idle");
        interactAnim.Play("Open");
    }

    private void OnDisable()
    {
        interactAnim.Play("Close");
        rb.isKinematic = false;
    }

    private void Update()
    {
        if(Input.GetButtonDown("NPC"))
        {
            if (GameManager.Instance.DialogueManager.isDialogueActive)
            {
                GameManager.Instance.DialogueManager.AdvanceDialogue();
            }
            else
            {
                if (!GameManager.Instance.DialogueManager.CanStartDialogue())
                {
                    CheckForNewConversation();
                    GameManager.Instance.DialogueManager.StartDialogue(currentConversation);

                }
            }
        }
    }

    private void CheckForNewConversation()
    {
        for (int i = 0; i < conversations.Count; i++)
        {
            var convo = conversations[i];
            if(convo != null && convo.IsConditionMet())
            {
                currentConversation = convo;

                // remove one time convo

                if(convo.removeAfterComplete)
                    conversations.RemoveAt(i);

                // remove if quest complete.
                if(convo.removeTheseOnComplete!= null && convo.removeTheseOnComplete.Count > 0)
                {
                    foreach (var toRemove in convo.removeTheseOnComplete)
                    {
                        conversations.Remove(toRemove);
                    }
                }
                currentConversation = convo;
                break;
            }
        }
    }

    private void OnQuestAccepted_RemovedOfferings(QuestSO accceptedQuest)
    {
        for (int i = conversations.Count - 1; i >= 0; i--)
        {
            var convo = conversations[i];
            if(convo == null)
                continue;

            if (convo.offerQuestOnEnd == accceptedQuest)
                conversations.RemoveAt(i);

    
        }
    }

}
