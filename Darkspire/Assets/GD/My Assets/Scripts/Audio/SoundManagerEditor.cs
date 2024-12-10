using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector(); // Draw default Inspector layout

    //    SoundManager soundManager = (SoundManager)target;

    //    if (GUILayout.Button("Update Sound Names"))
    //    {
    //        soundManager.SendMessage("UpdateSoundList", null, SendMessageOptions.DontRequireReceiver);
    //        EditorUtility.SetDirty(soundManager); // Mark as dirty
    //        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(soundManager.gameObject.scene); // Mark scene dirty
    //    }
    //}
}
