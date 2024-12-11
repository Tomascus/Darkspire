using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Gate gate; // Serialize the field to assign in the Inspector
    public bool playerInRange = false;
    private bool pressed = false;
    [SerializeField]private AudioSource audioSource;

    //private void Update()
    //{
    //    if (playerInRange && Input.GetKeyDown(KeyCode.E))
    //    {
    //        PressLever();
    //    }
    //}

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

    public void PressLever()
    {
        if(!pressed)
        {
            if (gate != null)
            {
                SoundManager.PlaySound(SoundType.GATE_OPEN, audioSource);
                gate.StartMoving();
                pressed = true;
            }
        }
        
    }
}