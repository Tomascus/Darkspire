using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("TestScene");
        MusicManager.Instance.PlayMusic("Gameplay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
