using UnityEngine;

public class PlayCatFishSounds : MonoBehaviour
{
    public void PlayCatFishWalk()
    {
        SoundManager.PlaySound(SoundType.FISHFOOTSTEP);
    }

    public void PlayFishAttack()
    {
        SoundManager.PlaySound(SoundType.FISHSWING);
    }

    public void PlayFishOverHead()
    {
        SoundManager.PlaySound(SoundType.FISHOVERHEADSWING);
    }

    public void PlayFishHit()
    {
        SoundManager.PlaySound(SoundType.FISHHIT);
    }

    public void PlayFishDied()
    {
        SoundManager.PlaySound(SoundType.FISHDIED);
    }
}
