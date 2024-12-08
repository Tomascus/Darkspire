using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    #region Play Sounds for Menus
    public void PlayHoverSoundMenu()
    {
        SoundManager.PlayMenuSound(SoundType.MENUHOVER);
    }

    public void PlayClickSound()
    {
        SoundManager.PlayMenuSound(SoundType.MENUCLICK);
    } 
    #endregion
}
