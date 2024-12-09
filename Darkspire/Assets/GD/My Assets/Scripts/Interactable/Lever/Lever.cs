using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Gate gate; // Serialize the field to assign in the Inspector
    private bool playerInRange = false;
    private bool pressed = false;
    [SerializeField]private AudioSource audioSource;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PressLever();
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
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void PressLever()
    {
        if(!pressed)
        {
            if (gate != null)
            {
                SoundManager.PlaySound(SoundType.GATEOPEN, audioSource);
                gate.StartMoving();
                pressed = true;
            }
        }
        
    }
}