using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DollyCameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera; // Player camera
    public CinemachineVirtualCamera dollyCamera;  // Cuscene camera
    public CinemachineDollyCart dollyCart;        // Cutscene dolly cart - cinemamachine component
    [SerializeField] private float cutsceneCameraSpeed = 2f;        // Speed of the dolly cart
    [SerializeField] private float cutsceneLength = 10f;            // Duration of the dolly track animation 
    [SerializeField] private float movementDelay = 0.5f;            // Disable movement after a delay to allow the player to come to stop 
    [SerializeField] private string loadGameScene;                  


    private bool isCutscenePlaying = false;
    private bool cutsceneDone = false; // Makes sure that the cutscene can be played only once

    private PlayerMovementController playerMovementController;

    private void Start()
    {
        // Get the PlayerMovementController component from the player for stopping movement
        playerMovementController = FindObjectOfType<PlayerMovementController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCutscenePlaying && !cutsceneDone)
        {
            StartCoroutine(PlayCutscene());
        }
    }

    private IEnumerator PlayCutscene()
    {
        isCutscenePlaying = true;
        cutsceneDone = true;

        yield return new WaitForSeconds(movementDelay);
        playerMovementController.SetMovementEnabled(false);

        // Switch to dolly camera using priotity system of cinemamachine
        playerCamera.Priority = 0;  
        dollyCamera.Priority = 10; 

        // Start the dolly cart movement
        dollyCart.m_Speed = cutsceneCameraSpeed;

        // Wait for the duration of the dolly animation 
        yield return new WaitForSeconds(cutsceneLength);

        // Switch back to player camera using priotity system of cinemamachine
        dollyCamera.Priority = 0;  // Lower the priority of the dolly camera
        playerCamera.Priority = 10; // Raise the priority of the player camera

        // Enable player movement
        if (playerMovementController != null)
        {
            playerMovementController.SetMovementEnabled(true);
        }

        // Loadd the game scene
        if (!string.IsNullOrEmpty(loadGameScene))
        {
            SceneManager.LoadScene(loadGameScene);
        }

        isCutscenePlaying = false;
    }
}
