using UnityEngine;

public class PlayStoneGolemSound : MonoBehaviour
{

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void GolemWalk()
    {
        SoundManager.PlaySound(SoundType.STONEGOLEM_WALK, audioSource);
    }

    void GolemSwing()
    {
        SoundManager.PlaySound(SoundType.STONEGOLEM_SWING, audioSource);
    }

    void GolemHit()
    {
        SoundManager.PlaySound(SoundType.STONEGOLEM_HIT, audioSource);
    }

    void GolemStomp()
    {
        SoundManager.PlaySound(SoundType.STONEGOLEM_STOMP, audioSource);
    }
}
