using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNPCTalk : MonoBehaviour
{
    public void PlayIntroMerchant()
    {
        SoundManager.PlaySound(SoundType.TALK);
    }
}
