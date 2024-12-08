using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default Inspector layout

        SoundManager soundManager = (SoundManager)target;

        if (GUILayout.Button("Update Sound Names"))
        {
            soundManager.SendMessage("UpdateSoundList", null, SendMessageOptions.DontRequireReceiver);
            EditorUtility.SetDirty(soundManager); // Mark as dirty to save changes
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(soundManager.gameObject.scene); // Mark the scene dirty
        }
    }
}
