using UnityEngine;

public class PlayFootstep : MonoBehaviour
{

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //All player movement sounds are here. The rest are in the animator as it allows different sounds for different animations of attacks

    #region Playing Sound for Player
    public void PlayFootStompSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERFOOTSTEP, audioSource);
    }

    public void PlaySprintingSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERRUN, audioSource);
    }
    public void PlayDodgeSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERDODGE, audioSource);
    }
    #endregion
}
