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
    public bool isOpen = false;
    public bool playerInRange = false;
    private Quaternion openRotation;
    private Quaternion closedRotation;

    private AudioSource audioSource;
    private AudioSource audioForKeyTwist;
    private void Start()
    {
        closedRotation = doorMesh.localRotation; // Get the initial rotation of the door
        openRotation = Quaternion.Euler(doorMesh.localEulerAngles + new Vector3(0, openAngle, 0)); // Calculate the open rotation of the door
        audioSource = GetComponent<AudioSource>();

        Transform doorTwist = transform.Find("DoorAudioSource");    //This code allows for sound to play on the door when key is put in, Audio Sources only allow one sound to play at a time
        if (doorTwist != null)
        {
            audioForKeyTwist = doorTwist.GetComponent<AudioSource>();
            if (audioForKeyTwist == null)
            {
                Debug.LogError("Audio Source not found for Key twist. Add componenet for It");
            }
        }
        else
        {
            Debug.LogError($"'DoorAudioSource' GameObject not found as a child of {gameObject.name}. Check the hierarchy and name.");
        }
    }

    public void RotateDoor()
    {
        if (isOpen)
        {
            doorMesh.localRotation = Quaternion.Slerp(doorMesh.localRotation, openRotation, Time.deltaTime * openSpeed);
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

    public void TryOpenDoor()
    {
        if (playerInventory.Count(keyItem) > 0) //will open door if player has a key in inventory 
        {
            SoundManager.PlaySound(SoundType.KEYTWIST, audioForKeyTwist);    //PLay sound for when key is used
            playerInventory.Remove(keyItem, 1);
            isOpen = true;
            SoundManager.PlaySound(SoundType.DOOROPEN, audioSource);    //Sound of the door opening
        }
        else
        {
            noKeyImage.gameObject.SetActive(true); //if player does not have key, show no key image
        }
    }
    
}

