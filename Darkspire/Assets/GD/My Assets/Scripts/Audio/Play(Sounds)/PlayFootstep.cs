using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
   public void PlayFootstepSound()
    {
        SoundManager.PlaySound(SoundType.FOOTSTEP);
    }

   public void PlaySprintSound()
   {
        SoundManager.PlaySound(SoundType.RUN);
   }
}
