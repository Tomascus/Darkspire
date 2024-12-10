using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNPCTalk : MonoBehaviour
{

    private AudioSource audioSource;

    #region Play Sounds for NPC
    public void PlayIntroMerchant()
    {
        SoundManager.PlaySound(SoundType.MERCHANTTALK, audioSource);
    } 
    #endregion
}
