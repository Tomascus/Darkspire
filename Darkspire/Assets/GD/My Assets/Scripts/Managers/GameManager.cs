using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Scene Loading Methods
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) //Play music based on Scene
    {
        if (scene.name == "MainMenu")
        {
            MusicManager.Instance.PlayMusic("MainMenu");
        }
        else if(scene.name =="Intro")
        {
            MusicManager.Instance.PlayMusic("IntroMusic");
        }
        else if (scene.name == "MapPrototype")
        {
            MusicManager.Instance.PlayMusic("Gameplay");
        }

    }
    #endregion
}