using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ThemeSong : MonoBehaviour
{

    // Configuration Parameters
    [SerializeField] AudioSource mainTheme;

    // Build Scene Index
    int currentSceneIndex;

    private void Awake()
    {
        SetUpSingleton();
    }

    void Update()
    {
        // Grabs the number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if( currentSceneIndex == 3 || currentSceneIndex == 9 ) // Fade out on loading screen
        {
            StartCoroutine(FadeAudioSource.StartFade(mainTheme, 3f, 0));
        }

        if( currentSceneIndex == 4 || currentSceneIndex == 10 ) // Destroy it on game screen
        {
            Destroy(gameObject);
        }
    }

    private void SetUpSingleton()
    {
        // If there is already a music object for the main theme, do not start a new one
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Not being used yet - 5/24/2020
    public void PauseStartMenuSong()
    {
        mainTheme.Pause();
        Debug.Log("Paused");
    }

    // Not being used yet - 5/24/2020
    public void PlayStartMenuSong()
    {
        mainTheme.Play();
        Debug.Log("Play");
    }
    
}

public static class FadeAudioSource {

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}