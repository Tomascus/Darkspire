using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    //All player movement sounds are here. The rest are in the animator as it allows different sounds for different animations of attacks

    #region Playing Sound for Player
    public void PlayFootStompSound()
    {
        SoundManager.PlaySound(SoundType.FOOTSTEP);
    }

    public void PlaySprintingSound()
    {
        SoundManager.PlaySound(SoundType.RUN);
    }
    public void PlayDodgeSound()
    {
        SoundManager.PlaySound(SoundType.DODGE);
    }
    #endregion
}
