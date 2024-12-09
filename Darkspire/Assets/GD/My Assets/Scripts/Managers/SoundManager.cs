using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private SoundList[] soundList; // Holds all sound lists with clips
    public static SoundManager instance;

    private Dictionary<string, AudioClip> soundDictionary;
    #endregion

    #region Unity Built-in Methods
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
    public static void PlaySound(SoundType sound, AudioSource audioSource, float volume = 1f)
    {
        if (instance == null)
        {
            Debug.LogError("SoundManager instance is null! Make sure a SoundManager exists in the scene.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is null! Ensure the calling GameObject has an AudioSource.");
            return;
        }

        SoundList soundList = instance.soundList[(int)sound];
        AudioClip[] clips = soundList.Sounds;

        if (clips == null || clips.Length == 0)
        {
            Debug.LogError($"No audio clips assigned in sound list: {soundList.name}");
            return;
        }

        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        // Randomize pitch and volume to prevent sound fatigue
        float randomPitch = UnityEngine.Random.Range(0.95f, 1.05f);
        float randomVolume = volume * UnityEngine.Random.Range(0.95f, 1.05f);

        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(randClip, randomVolume);
    }

    public static void PlayMenuSound(SoundType soundMenu, AudioSource audioSource)
    {
        if (instance == null)
        {
            Debug.LogError("SoundManager instance is null! Make sure a SoundManager exists in the scene.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is null! Ensure the calling GameObject has an AudioSource.");
            return;
        }

        SoundList soundList = instance.soundList[(int)soundMenu];
        AudioClip[] clips = soundList.Sounds;

        if (clips == null || clips.Length == 0)
        {
            Debug.LogError($"No audio clips assigned in sound list: {soundList.name}");
            return;
        }

        AudioClip randClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        audioSource.pitch = 1.0f; // Reset pitch
        audioSource.PlayOneShot(randClip, 1.0f); // Play at full volume
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
    #endregion
    #region Updating List

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
                    name = names[i],
                    sounds = soundList[i].sounds ?? new AudioClip[0], // Preserve sounds or initialize
                };
            }
        }

        Debug.Log("SoundList updated successfully!");
    }
#endif
}

#endregion


[Serializable]
public struct SoundList
{
    public string name;
    public AudioClip[] sounds;

    public AudioClip[] Sounds => sounds;
}