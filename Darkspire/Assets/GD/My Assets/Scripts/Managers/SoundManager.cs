using UnityEngine;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList; // Holds all sounds
    public static SoundManager instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (!Application.isPlaying) return;

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        UpdateSoundList();
    }

    private void OnValidate()
    {
        UpdateSoundList();
    }

    private void UpdateSoundList()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < names.Length; i++)
        {
            if (soundList[i].name != names[i])
            {
                soundList[i] = new SoundList
                {
                    name = names[i], // Assign the enum name to the SoundList's name
                    sounds = soundList[i].Sounds // Preserve existing sound references
                };
            }
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public string name;
    [SerializeField] public AudioClip[] sounds;

    public AudioClip[] Sounds => sounds;
}