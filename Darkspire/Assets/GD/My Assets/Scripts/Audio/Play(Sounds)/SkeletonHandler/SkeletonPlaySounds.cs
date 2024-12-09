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
        SoundManager.PlaySound(SoundType.SKELETONWALK, audioSource);
    }

    public void SkeletonAttack() 
    { 
        SoundManager.PlaySound(SoundType.PLAYERSWING, audioSource);
    }
}