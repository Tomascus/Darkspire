using UnityEngine;
using System;

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
    HOVER,
    CLICK
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    public static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //instance = this;
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randClip, volume);
    }

    

    #if UNITY_EDITOR
    private void OnEnable()
    {
        //Names based on enum names
        string[] names = Enum.GetNames(typeof(SoundType));

        //Change name bassed on enum
        Array.Resize(ref soundList, names.Length);

        //Set name of sound
        for (int i = 0; i < names.Length; i++)
        {
            soundList[i].name = names[i];
            
        }
    }
    #endif
}


[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    public string name;
    [SerializeField] private AudioClip[] sounds;

}
