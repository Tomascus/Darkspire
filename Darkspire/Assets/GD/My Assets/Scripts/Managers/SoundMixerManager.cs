using UnityEngine;
using UnityEngine.Audio;

//Referance for script : https://www.youtube.com/watch?v=DU7cgVsU2rM

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance;


    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);

    }
}
