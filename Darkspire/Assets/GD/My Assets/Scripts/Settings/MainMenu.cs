using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //Once play button is clicked, load the MapPrototype scene
    public void PlayGame()
    {
        SceneManager.LoadScene("Intro");
    }
    //Once quit button is clicked, quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
