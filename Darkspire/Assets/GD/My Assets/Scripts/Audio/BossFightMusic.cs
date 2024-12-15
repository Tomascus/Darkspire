using UnityEngine;

public class BossFightMusic : MonoBehaviour
{
    [SerializeField] private string bossName = "Stone Golem";
    [SerializeField] private string bossMusic = "BossMusic";
    private string previousMusic;
    private bool isBossMusicPlaying = false;
    private BossController bossScript; // Reference to the Boss script

    private void Start()
    {
        GameObject bossObject = GameObject.Find(bossName);

        if (bossObject != null)
        {
            bossScript = bossObject.GetComponent<BossController>(); // Assuming Boss script handles the 'isDead' flag
            if (bossScript == null)
            {
                Debug.LogError("Boss script not found on the boss object!");
            }
        }
        else
        {
            Debug.LogError("Boss not found!");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && bossScript != null && !isBossMusicPlaying)
        {
            if (!bossScript.isBossDead()) // Check if the boss is alive
            {
                // Save the currently playing music
                previousMusic = MusicManager.Instance.GetCurrentMusicName();

                // Play the boss music
                MusicManager.Instance.PlayMusic(bossMusic);
                isBossMusicPlaying = true;

                // Start checking if the boss is defeated
                StartCoroutine(CheckBossDefeated());
            }
        }
    }

    private System.Collections.IEnumerator CheckBossDefeated()
    {
        while (!bossScript.isBossDead()) // Use isDead flag instead of activeSelf
        {
            yield return null; // Wait until the next frame
        }


        // Boss is defeated, stop boss music and restart previous music
        MusicManager.Instance.StopMusic();

        if (!string.IsNullOrEmpty(previousMusic))
        {
            MusicManager.Instance.PlayMusic(previousMusic);
        }

        isBossMusicPlaying = false;
    }
}