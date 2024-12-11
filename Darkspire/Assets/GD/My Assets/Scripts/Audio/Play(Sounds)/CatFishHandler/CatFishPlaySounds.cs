using UnityEngine;

//Reason for this script to exist is as Animation for walking is Read-only

public class CatFishPlaySounds : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    public void FishWak()
    {
        SoundManager.PlaySound(SoundType.FISH_FOOTSTEP,audioSource);
    }

    public void FishWalk()
    {

        SoundManager.PlaySound(SoundType.FISH_FOOTSTEP, audioSource);
    }

    public void FishAttack()
    {
        SoundManager.PlaySound(SoundType.FISH_SWING, audioSource);
    }

    public void FishOverHead()
    {
        SoundManager.PlaySound(SoundType.FISH_OVERHEADSWING, audioSource);
    }

    public void FishHit()
    {
        SoundManager.PlaySound(SoundType.FISH_HIT, audioSource);
    }

    public void FishDied()
    {
        SoundManager.PlaySound(SoundType.FISH_DIED, audioSource);
    }


}
