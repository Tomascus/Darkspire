using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    #region Play Sounds for Menus
    public void PlayHoverSoundMenu()
    {
        SoundManager.PlaySound(SoundType.HOVER);
    }

    public void PlayClickSound()
    {
        SoundManager.PlaySound(SoundType.CLICK);
    } 
    #endregion
}
