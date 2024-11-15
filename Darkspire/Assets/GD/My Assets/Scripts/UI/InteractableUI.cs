using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    [Tooltip("UI to show when player is in range.")]
    [SerializeField] private GameObject interactableUI;

    private bool playerInRange = false;

    private void Awake()
    {
        interactableUI.SetActive(false); //make sure not to display when start the game
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //when player gets in range display the UI 
        {
            playerInRange = true;
            if (interactableUI != null)
            {
                interactableUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false; //when player is out of range hide the UI
        if (interactableUI != null)
        {
            interactableUI.SetActive(false);
        }
    }

}
