using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using InputSystem;

public class NPCDialogue : MonoBehaviour
{

    public bool playerInRange = false;
    [SerializeField] private NPCConversation dialogueNPC; // access the conversation asset created 
    private PlayerControllerInputs playerControllerInputs; // access the player controller inputs script
    private bool inDialogue = false;

    private void Awake()
    {
        playerControllerInputs = FindObjectOfType<PlayerControllerInputs>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerControllerInputs.inDialogue = true; // set attacking off
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        playerControllerInputs.inDialogue = false;
    }

    public void StartDialogue()
    {
        if (!inDialogue)
        {
            inDialogue = true;
            ConversationManager.Instance.StartConversation(dialogueNPC); // start the conversation, create instance of the conversation manager
            playerControllerInputs.ShowCursor(); // show cursor for dialogue
        }
    }

    public void EndDialogue()
    {
        inDialogue = false;
        playerControllerInputs.HideCursor(); // hide cursor after dialogue
    }
}
