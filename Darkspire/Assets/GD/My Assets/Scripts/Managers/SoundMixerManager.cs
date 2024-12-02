using UnityEngine;
using UnityEngine.Audio;
using Slider = UnityEngine.UI.Slider;

//Referance for script : https://www.youtube.com/watch?v=DU7cgVsU2rM

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;

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

        LoadPreferences();

        Debug.Log($"MasterVolume Loaded: {masterSlider}");
        Debug.Log($"MusicVolume Loaded: {musicSlider}");
        Debug.Log($"SoundFXVolume Loaded: {soundFXSlider}");

    }

    private void LoadPreferences()          //Methods to check if the player has set the volume before, if has then set to save variable, if hasn't then set to default

    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
            Debug.Log($"Loaded MasterVolume: {masterVolume}");
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20f);
            if (masterSlider != null) masterSlider.value = masterVolume;    //Updating slider
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20f);
            if (musicSlider != null) musicSlider.value = musicVolume;   // Updating slider
        }

        if (PlayerPrefs.HasKey("SoundFXVolume"))
        {
            float soundFXVolume = PlayerPrefs.GetFloat("SoundFXVolume", 0.8f);
            audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(soundFXVolume) * 20f);
            if (soundFXSlider != null) soundFXSlider.value = soundFXVolume; // Updating slider
        }
    }

    public void SetGameVolume(float volume)
    {
        AudioListener.volume = volume; 
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
        
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", level); 
        PlayerPrefs.Save();
    }
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("SoundFXVolume", level);
        PlayerPrefs.Save();
        Debug.Log($"Saved SFXSOUND: {level}");
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", level);
        PlayerPrefs.Save();

    }
}
