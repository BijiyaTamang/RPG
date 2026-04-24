using UnityEngine;
using System;
public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactAnim;
    public DialogueSO dialogueSO;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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
            if (DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else
            {
                DialogueManager.Instance.StartDialogue(dialogueSO);
            }
        }
    }
}
