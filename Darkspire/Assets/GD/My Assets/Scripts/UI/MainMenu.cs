using UnityEngine;
using UnityEngine.SceneManagement;

//Structure got at : https://www.youtube.com/watch?v=-GWjA6dixV4

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // all examples load 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
