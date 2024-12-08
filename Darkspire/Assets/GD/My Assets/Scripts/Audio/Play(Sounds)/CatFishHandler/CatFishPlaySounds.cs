using UnityEngine;

//Reason for this script to exist is as Animation for walking is Read-only

public class CatFishPlaySounds : MonoBehaviour
{
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();

    }


    public void FishWak()
    {
        Debug.Log("FishWalk triggered!");
        SoundManager.PlaySound(SoundType.SKELETONWALK);
    }

    public void FishWalk()
    {
        Debug.Log("FishWalk triggered!");
        SoundManager.PlaySound(SoundType.SKELETONWALK);
    }

    public void FishAttack()
    {
        Debug.Log("FishAttack triggered!");
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
