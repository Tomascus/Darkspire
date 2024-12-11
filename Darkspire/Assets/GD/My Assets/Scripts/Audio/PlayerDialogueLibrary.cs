using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public struct DialogueList
{
    [Tooltip("Name of the ItemToPickUp")]
    public string ItemName;

    [Tooltip("Dialogue Text")]
    public AudioClip clip;
}


public class PlayerDialogueLibrary : MonoBehaviour
{
    public DialogueList[] Items;
    private bool isPlaying = false;

    public AudioClip GetClipFromName(string ItemName)
    {
        foreach (var track in Items)
        {
            if (track.ItemName == ItemName)
            {
                return track.clip;
            }
        }
        return null;
    }
}
