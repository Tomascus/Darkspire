using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Fields

    public GameObject pauseMenu;
    public static bool isPaused;
    private PlayerControllerInputs playerControllerInputs;
    #endregion

    #region Unity Method Codes
    private void Start()
    {
        
        pauseMenu.SetActive(false);     //turn off the pause object
        playerControllerInputs = FindObjectOfType<PlayerControllerInputs>();    //find the player controller inputs script
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //Pauses game once escape is pressed or unpause
        {
            //
            if (isPaused)
            {
                playerControllerInputs.HideCursor();
                ResumeGame();


            }
            else
            {

                PauseGame();
                playerControllerInputs.ShowCursor();
            }
        }

    }
    #endregion

    #region Funtionality of Pause Menu Buttons
    public void PauseGame()
    {
        pauseMenu.SetActive(true); //Turn on the object
        Time.timeScale = 0f;    //Stops time in the game(in turn stoping everything in the game)
        isPaused = true;    //Cant do anything other than pause menu
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false); //Turn off the object
        Time.timeScale = 1f;    //Resumes time in the game
        isPaused = false;   //Cant do anything other than pause menu
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;    //Start the time
        SceneManager.LoadScene("MainMenu"); //Go back to the main menu
    }

    public void QuitGame()
    {
        Application.Quit(); //Quit the game
    } 
    #endregion

}
