#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerDialogueLibrary))]
public class PlayerDialogueLibraryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerDialogueLibrary library = (PlayerDialogueLibrary)target;

        if (GUILayout.Button("Populate Dialogue Library"))
        {
            PopulateLibrary(library);
        }
    }

    private void PopulateLibrary(PlayerDialogueLibrary library)
    {
        // Create a temporary dictionary to map ItemName to AudioClip
        Dictionary<string, AudioClip> existingClips = new Dictionary<string, AudioClip>();

        // Preserve existing AudioClip references
        foreach (var item in library.Items)
        {
            if (!string.IsNullOrEmpty(item.ItemName) && item.clip != null)
            {
                existingClips[item.ItemName] = item.clip;
            }
        }

        // Find all ItemData assets in the project
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        DialogueList[] newItems = new DialogueList[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(path);

            if (itemData != null)
            {
                newItems[i] = new DialogueList
                {
                    ItemName = itemData.ItemName, // Assuming ItemData has an "ItemName" property
                    clip = existingClips.ContainsKey(itemData.ItemName)
                        ? existingClips[itemData.ItemName] // Preserve existing clip
                        : null // Otherwise, initialize with null
                };
            }
        }

        // Assign the updated array back to the library
        library.Items = newItems;

        EditorUtility.SetDirty(library); // Mark library as dirty so changes are saved
    }
}
#endif
