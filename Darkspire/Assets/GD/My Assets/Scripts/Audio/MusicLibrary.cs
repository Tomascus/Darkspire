using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    [Tooltip("Music Name to Access from Code")]
    public string trackName;

    [Tooltip("Name of Songs->(No Title Necessary for Implementation)")]
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;

    public AudioClip GetClipFromName(string trackName)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == trackName)
            {
                return track.clip;
            }
        }
        return null;
    }
}
