using UnityEngine;

public class PlaySkeletonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void PlaySkeletonWalk()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        SoundManager.PlaySound(SoundType.SKELETONWALK);
    }

}
