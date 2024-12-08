using UnityEngine;

//Reason for this script to exist is as Animation for walking is Read-only

public class CatFishPlaySounds : MonoBehaviour
{
    private Animator animator;
    private PlayCatFishSounds fishSounds;

    void Start()
    {
        animator = GetComponent<Animator>();
        fishSounds = GetComponent<PlayCatFishSounds>();
    }


    public void FishWalk()
    {
        SoundManager.PlaySound(SoundType.SKELETONWALK);
    }

    public void FishAttack()
    {
        SoundManager.PlaySound(SoundType.FISHSWING);
    }

    public void FishOverHead()
    {
        SoundManager.PlaySound(SoundType.FISHOVERHEADSWING);
    }

    public void FishHit()
    {
        SoundManager.PlaySound(SoundType.FISHHIT);
    }

    public void FishDied()
    {
        SoundManager.PlaySound(SoundType.FISHDIED);
    }


}
