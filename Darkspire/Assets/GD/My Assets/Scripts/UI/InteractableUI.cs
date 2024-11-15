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
            interactableUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            Debug.Log("Player is in range.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
        playerInRange = false;
        if (interactableUI != null)
        {
            interactableUI.SetActive(false);
        }
    }

}
