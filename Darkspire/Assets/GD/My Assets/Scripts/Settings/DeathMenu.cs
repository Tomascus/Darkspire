using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryScene : MonoBehaviour
{
    [SerializeField]private PlayerUI playerUl;

    public void RetryButton()
    {
        DOTween.KillAll();
        if (playerUl != null)
        {
            playerUl.ResetPlayerDeath(); // Ensure playerUI exists before calling ResetPlayerState
        }
        else
        {
            Debug.LogError("PlayerUI reference is missing in RetryButton.");
        }
        Time.timeScale = 1f;
        StartCoroutine(RestartScene());
        // Reset time scale to normal
        

    }


    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
