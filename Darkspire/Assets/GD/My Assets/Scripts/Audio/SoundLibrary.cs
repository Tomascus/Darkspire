using System.ComponentModel;
using UnityEngine;
[System.Serializable]
public struct SoundEffect
{
    [Tooltip("Name of Group to Call in Scripts")]
    public string groupID;

    [Tooltip("All Sound Posibility for Group")]
    public AudioClip[] clips;
}

public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.groupID == name)
            {
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
            }
        }

        return null;
    }

}

