using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private ItemData keyItem;
    [SerializeField] private float openAngle = 90.0f;
    [SerializeField] private float openSpeed = 1.0f;
    [SerializeField] private Transform doorMesh; // Reference to the door mesh
    [SerializeField] private Image noKeyImage;
    private bool isOpen = false;
    private bool playerInRange = false;
    private Quaternion openRotation;
    private Quaternion closedRotation;

    private void Start()
    {
        closedRotation = doorMesh.localRotation; // Get the initial rotation of the door
        openRotation = Quaternion.Euler(doorMesh.localEulerAngles + new Vector3(0, openAngle, 0)); // Calculate the open rotation of the door
    }

    private void Update()
    {
        if (isOpen) //do the rotation of the door
        {
            doorMesh.localRotation = Quaternion.Slerp(doorMesh.localRotation, openRotation, Time.deltaTime * openSpeed);
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.E)) //try open when player is in range and presses E, inventory check in function 
        {
            TryOpenDoor();
        }
    }

    private void OnTriggerStay(Collider other) //player is in range 
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }

        noKeyImage.gameObject.SetActive(false); //when leaving just turn off
    }

    private void TryOpenDoor()
    {
        if (playerInventory.Count(keyItem) > 0) //will open door if player has a key in inventory 
        {
            playerInventory.Remove(keyItem, 1);
            isOpen = true;
        }
        else
        {
            noKeyImage.gameObject.SetActive(true); //if player does not have key, show no key image
        }
    }
}

