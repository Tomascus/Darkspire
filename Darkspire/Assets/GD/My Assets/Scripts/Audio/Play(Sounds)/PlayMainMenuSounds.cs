using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    public void PlayHoverSoundMenu()
    {
        SoundManager.PlaySound(SoundType.HOVER);
    }

    public void PlayClickSound()
    {
        SoundManager.PlaySound(SoundType.CLICK);
    }
}
