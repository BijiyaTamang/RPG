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

    public string npcID;
    [SerializeField] private List<string> originalConversationNames = new List<string>();
    private bool hasInitialized = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        if (!hasInitialized)
        {
            InitializeOriginalConversations();
            hasInitialized = true;
        }
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
        if (GameManager.Instance != null)
            GameManager.Instance.SaveGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("NPC"))
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
            if (convo != null && convo.IsConditionMet())
            {
                currentConversation = convo;

                if (convo.removeAfterComplete)
                {
                    conversations.RemoveAt(i);
                    Debug.Log($"[{npcID}] Removed after complete: {convo.name}");
                }

                if (convo.removeTheseOnComplete != null && convo.removeTheseOnComplete.Count > 0)
                {
                    foreach (var toRemove in convo.removeTheseOnComplete)
                    {
                        if (toRemove == null)
                        {
                            Debug.LogWarning($"[{npcID}] Null entry in removeTheseOnComplete on '{convo.name}' — reassign in Inspector.");
                            continue;
                        }
                        conversations.Remove(toRemove);
                        Debug.Log($"[{npcID}] Removed these on complete: {toRemove.name}");
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
            if (convo == null)
                continue;
            if (convo.offerQuestOnEnd == accceptedQuest)
                conversations.RemoveAt(i);
        }
    }

    public void SaveDialogueState()
    {
        var removedNames = new List<string>();
        foreach (var name in originalConversationNames)
        {
            bool stillExists = conversations.Exists(c => c != null && c.name == name);
            if (!stillExists)
                removedNames.Add(name);
        }
        Debug.Log($"[{npcID}] Saving removed dialogues: {removedNames.Count} — " + string.Join(", ", removedNames));

        var existing = GameManager.Instance.playerData.npcDialogueStates
            .Find(x => x.npcID == npcID);
        if (existing != null)
            existing.removedDialogues = removedNames;
        else
            GameManager.Instance.playerData.npcDialogueStates.Add(new NPCDialogueData
            {
                npcID = npcID,
                removedDialogues = removedNames
            });
    }

    public void LoadDialogueState()
    {
        if (!hasInitialized)
        {
            InitializeOriginalConversations();
            hasInitialized = true;
        }

        if (GameManager.Instance == null || GameManager.Instance.playerData == null) return;
        var saved = GameManager.Instance.playerData.npcDialogueStates
            .Find(x => x.npcID == npcID);
        if (saved == null) { Debug.Log($"[{npcID}] No saved dialogue state found."); return; }
        Debug.Log($"[{npcID}] Loading removed dialogues: {saved.removedDialogues.Count} — " + string.Join(", ", saved.removedDialogues));

        for (int i = conversations.Count - 1; i >= 0; i--)
        {
            if (conversations[i] != null && saved.removedDialogues.Contains(conversations[i].name))
                conversations.RemoveAt(i);
        }
    }

    [ContextMenu("Initialize Original Conversations")]
    public void InitializeOriginalConversations()
    {
        originalConversationNames.Clear();
        foreach (var convo in conversations)
            if (convo != null)
                originalConversationNames.Add(convo.name);
        Debug.Log($"[{npcID}] Initialized {originalConversationNames.Count} original conversations.");
    }
}