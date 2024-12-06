using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    #region Playing Sound for Player
    public void PlayFootStompSound()
    {
        SoundManager.PlaySound(SoundType.FOOTSTEP);
    }

    public void PlaySprintingSound()
    {
        SoundManager.PlaySound(SoundType.RUN);
    }

    public void PlaySwinningSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERSWING);
    }

    public void PlayPokeSound()
    {
        SoundManager.PlaySound(SoundType.PLAYERPOKE);
    }

    public void PlayOverheadSound()
    {
        SoundManager.PlaySound(SoundType.PLAYEROVERHEAD);
    }
    public void PlayDodgeSound()
    {
        SoundManager.PlaySound(SoundType.DODGE);
    } 
    #endregion
}
