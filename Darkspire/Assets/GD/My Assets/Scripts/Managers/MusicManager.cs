using System.Collections;
using UnityEngine;

//refereance music manager : https://www.youtube.com/watch?v=Q-bKHocRvE0&t=55s

public class MusicManager : MonoBehaviour
{
    #region Fields
    public static MusicManager Instance;

    [Tooltip("Put Music Script from Inspector")]
    [SerializeField]
    private MusicLibrary musicLibrary;

    [Tooltip("Put Music Object from Hierarchy")]
    [SerializeField]
    private AudioSource musicSource;
    #endregion
    #region Unity In Built Methods
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
    }
    #endregion
    #region Music Playability

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));   //Play Music Based on scene
    }

    //Music That Can Change Flawlessly
    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    } 
    #endregion
}
