using UnityEngine;

//Reason for this script to exist is as Animation for walking is Read-only

public class SkeletonAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private PlaySkeletonSound skeletonSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        skeletonSound = GetComponent<PlaySkeletonSound>();
    }

    void Update()
    {
        // Check if the "Walk" animation is active
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SkeletonWalk"))
        {
            
        }
    }

    public void SkeletonWalk()
    {
        if (skeletonSound != null)
        {
            skeletonSound.PlaySkeletonWalk();
        }
    }
}