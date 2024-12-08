using UnityEngine;

//Reason for this script to exist is as Animation for walking is Read-only

public class SkeletonPlaySounds : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void SkeletonWalk()  //Calls animation event to trigger this method
    {
        Debug.Log("FishWalk triggered!");
        SoundManager.PlaySound(SoundType.SKELETONWALK);
    }

    public void SkeletonAttack() 
    { 
        SoundManager.PlaySound(SoundType.PLAYERSWING);
    }
}