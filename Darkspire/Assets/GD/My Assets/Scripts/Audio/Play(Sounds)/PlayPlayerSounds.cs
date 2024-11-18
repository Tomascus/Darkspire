using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
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
        SoundManager.PlaySound(SoundType.SWING);
    }
}
