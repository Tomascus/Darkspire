using UnityEngine;
using System;
using UnityEngine.SceneManagement;

//New updated sound system: https://www.youtube.com/watch?v=g5WT91Sn3hg

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    public static SoundManager instance;
    [SerializeField]private AudioSource audioSource;

    private void Awake()
    {
        if (!Application.isPlaying) return; // Makes it so the sound manager only runs in play mode

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
    //Trying to match the enum to the sound list
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

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
