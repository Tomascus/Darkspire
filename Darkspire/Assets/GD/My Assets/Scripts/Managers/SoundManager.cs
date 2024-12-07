using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private SoundList[] soundList; // Holds all sounds
    public static SoundManager instance;
    [SerializeField] private AudioSource audioSource;

    private Dictionary<string, AudioClip> soundDictionary;

    #endregion
    #region Unity In Built Settings
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
            InitializeSoundDictionary();
        }
    }
    #endregion

    #region Sound List Playability and Readability
    public static void PlaySound(SoundType sound, float volume = 1f)    //To play any sound in the list
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        // Randomize pitch and volume to prevent sound fatigue
        float randomPitch = UnityEngine.Random.Range(0.95f, 1.05f);
        float randomVolume = volume * UnityEngine.Random.Range(0.95f, 1.05f);

        instance.audioSource.pitch = randomPitch;
        instance.audioSource.PlayOneShot(randClip, randomVolume);
    }

    public static void PlayMenuSound(SoundType soundMenu)
    {
        AudioClip[] clips = instance.soundList[(int)soundMenu].Sounds;
        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        instance.audioSource.pitch = 1.0f; // Ensure pitch is set to default
        instance.audioSource.PlayOneShot(randClip, 1.0f); // Play at full volume
    }

    private void InitializeSoundDictionary()
    {
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var sound in soundList)
        {
            foreach (var clip in sound.Sounds)
            {
                soundDictionary[sound.name] = clip;
            }
        }
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

    private void UpdateSoundList()  //Update sound list when the enum is changed
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
#endregion

[Serializable]
public struct SoundList //Method for converting the enum to a list of sounds
{
    public string name;
    [SerializeField] public AudioClip[] sounds;
    public AudioClip[] Sounds => sounds;
}