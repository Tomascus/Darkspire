using UnityEngine;

//Reason for this script to exist is as Animation for walking is Read-only

public class SkeletonPlaySounds : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    public void SkeletonWalk()  //Calls animation event to trigger this method
    {
        SoundManager.PlaySound(SoundType.SKELETON_WALK, audioSource);
    }

    public void SkeletonAttack() 
    { 
        SoundManager.PlaySound(SoundType.PLAYER_SWING, audioSource);
    }

    public void SkeletonDied()
    {
        SoundManager.PlaySound(SoundType.SKELETON_DIED, audioSource);
    }

    public void SkeletonHit()
    {
        SoundManager.PlaySound(SoundType.SKELETON_HIT, audioSource);
    }
}