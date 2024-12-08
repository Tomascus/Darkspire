using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private SoundList[] soundList; // Holds all sound lists with sources
    public static SoundManager instance;

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
        SoundList soundList = instance.soundList[(int)sound];
        if (soundList.audioSource == null)
        {
            Debug.LogError($"No AudioSource assigned for sound list: {soundList.name}");
            return;
        }

        AudioClip[] clips = soundList.Sounds;
        if (clips.Length == 0)
        {
            Debug.LogError($"No audio clips assigned in sound list: {soundList.name}");
            return;
        }

        AudioSource selectedAudioSource = soundList.audioSource[UnityEngine.Random.Range(0, soundList.audioSource.Count)];

        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        // Randomize pitch and volume to prevent sound fatigue
        float randomPitch = UnityEngine.Random.Range(0.95f, 1.05f);
        float randomVolume = volume * UnityEngine.Random.Range(0.95f, 1.05f);

        selectedAudioSource.pitch = randomPitch;
        selectedAudioSource.PlayOneShot(randClip, randomVolume);
    }

    public static void PlayMenuSound(SoundType soundMenu)
    {
        SoundList soundList = instance.soundList[(int)soundMenu];
        if (soundList.audioSource == null)
        {
            Debug.LogError($"No AudioSource assigned for sound list: {soundList.name}");
            return;
        }

        AudioClip[] clips = soundList.Sounds;
        if (clips.Length == 0)
        {
            Debug.LogError($"No audio clips assigned in sound list: {soundList.name}");
            return;
        }

        AudioSource selectedAudioSource = soundList.audioSource[UnityEngine.Random.Range(0, soundList.audioSource.Count)];

        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        selectedAudioSource.pitch = 1.0f; // Ensure pitch is set to default
        selectedAudioSource.PlayOneShot(randClip, 1.0f); // Play at full volume
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

    private void UpdateSoundList()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        if (soundList == null)
        {
            Debug.LogError("SoundList is null!");
            return;
        }

        for (int i = 0; i < names.Length; i++)
        {
            // Initialize the list if it's null
            if (soundList[i].audioSource == null)
            {
                soundList[i].audioSource = new List<AudioSource>();
            }

            if (soundList[i].name != names[i])
            {
                soundList[i] = new SoundList
                {
                    name = names[i], // Assign the enum name to the SoundList's name
                    sounds = soundList[i].Sounds, // Preserve existing sound references
                    audioSource = soundList[i].audioSource // Preserve the list of audio sources
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
    public AudioClip[] sounds;

    public List<AudioSource> audioSource;   //Allows each list to have their own sound source. Making it easier to distinguish from Diegetic or Non-Diegetic
    public AudioClip[] Sounds => sounds;
}