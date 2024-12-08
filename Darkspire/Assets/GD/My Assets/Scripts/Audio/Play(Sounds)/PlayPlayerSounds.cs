using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    //All player movement sounds are here. The rest are in the animator as it allows different sounds for different animations of attacks

    #region Playing Sound for Player
    public void PlayFootStompSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERFOOTSTEP);
    }

    public void PlaySprintingSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERRUN);
    }
    public void PlayDodgeSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERDODGE);
    }
    #endregion
}
