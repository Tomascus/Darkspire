using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNPCTalk : MonoBehaviour
{
    #region Play Sounds for NPC
    public void PlayIntroMerchant()
    {
        SoundManager.PlaySound(SoundType.TALK);
    } 
    #endregion
}
