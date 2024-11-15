using System;
using UnityEditor;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [Tooltip("Get Script from Inspector of AudioManager")]
    [SerializeField]
    private SoundLibrary sfxLibrary;

    [Tooltip("Object for Activation of Sounds, Get from Hierarchy")]
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
    }

    //Any UI eg Healing or Damage
    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }

    internal void Stop( )
    {
        sfx2DSource.Stop();
    }
}
