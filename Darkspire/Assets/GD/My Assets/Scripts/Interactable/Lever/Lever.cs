using UnityEngine;

public class Lever : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private Gate gate; // Serialize the field to assign in the Inspector
    public bool playerInRange = false;
    private bool pressed = false;
    [SerializeField]private AudioSource audioSource;
    #endregion

    #region COLLISION
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
    #endregion

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