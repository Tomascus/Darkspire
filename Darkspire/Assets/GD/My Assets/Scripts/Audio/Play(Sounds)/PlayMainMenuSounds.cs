using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    #region Play Sounds for Menus
    public void PlayHoverSoundMenu()
    {
        SoundManager.PlayMenuSound(SoundType.HOVER);
    }

    public void PlayClickSound()
    {
        SoundManager.PlayMenuSound(SoundType.CLICK);
    } 
    #endregion
}
