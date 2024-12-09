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
        SoundManager.PlaySound(SoundType.FISHFOOTSTEP,audioSource);
    }

    public void FishWalk()
    {

        SoundManager.PlaySound(SoundType.FISHFOOTSTEP, audioSource);
    }

    public void FishAttack()
    {
        SoundManager.PlaySound(SoundType.FISHSWING, audioSource);
    }

    public void FishOverHead()
    {
        SoundManager.PlaySound(SoundType.FISHOVERHEADSWING, audioSource);
    }

    public void FishHit()
    {
        SoundManager.PlaySound(SoundType.FISHHIT, audioSource);
    }

    public void FishDied()
    {
        SoundManager.PlaySound(SoundType.FISHDIED, audioSource);
    }


}
