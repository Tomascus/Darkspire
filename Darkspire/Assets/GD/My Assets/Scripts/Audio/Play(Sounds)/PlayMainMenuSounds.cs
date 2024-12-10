using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    #region Play Sounds for Menus
    public void PlayHoverSoundMenu()
    {
        SoundManager.PlayMenuSound(SoundType.MENUHOVER, audioSource);
    }

    public void PlayClickSoundMenu()
    {
        SoundManager.PlayMenuSound(SoundType.MENUCLICK, audioSource);
    } 
    #endregion


}
