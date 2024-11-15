using UnityEngine;
using UnityEngine.Rendering;

//New updated sound system: https://www.youtube.com/watch?v=g5WT91Sn3hg

public enum SoundType
{
    SWING,
    HIT,
    DODGE,
    TALK,
    FOOTSTEP,
    RUN,
    INTERACTING,
    NO_STAMINA,
    LOW_HEALTH,


}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    public static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent< AudioSource >();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
