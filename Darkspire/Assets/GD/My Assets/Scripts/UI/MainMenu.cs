using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        MusicManager.Instance.PlayMusic("Gameplay");
        SceneManager.LoadScene("TestScene");
        
    }

    

    public void QuitGame()
    {
        Application.Quit();
    }
}
