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

    private void Awake()
    {
        playerControllerInputs = FindObjectOfType<PlayerControllerInputs>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ConversationManager.Instance.StartConversation(dialogueNPC); //start the conversation when in ragne
            playerControllerInputs.ShowCursor(); // show cursor for dialogue 
            playerControllerInputs.inDialogue = true; // set attacking off
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    { 
         playerInRange = false;
        playerControllerInputs.inDialogue = false;
    }
}