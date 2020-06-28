using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Configuration Parameters
    int currentSceneIndex;
    float timeToWait = 1.3f;
    int timeToStartGame = 15;

    // Start is called before the first frame update
    void Start()
    {
        // Grabs the number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // If we are at the splash screen, do coroutine
        if (currentSceneIndex == 0)
        {
            StartCoroutine( SplashToStart() );
        }

        // If on loading screen
        if (currentSceneIndex == 3)
        {
            StartCoroutine( LoadToGame() );
        }

        if (currentSceneIndex == 9)
        {
            StartCoroutine( LoadToHoopsGame() );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* --------- SOCCER ---------- ( Scenes 2-7 ) */

    public void LoadStartScreen()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadCarSelectScreen()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(4);
    }

    IEnumerator LoadToGame()
    {
        // Stuff to happen before
        yield return new WaitForSeconds(timeToStartGame);
        
        // Stuff to happen after
        LoadGame();
    }

    IEnumerator LoadToHoopsGame()
    {
        // Stuff to happen before
        yield return new WaitForSeconds(timeToStartGame);
        
        // Stuff to happen after
        LoadHoopsGame();
    }

    /* --------- HOOPS ---------- ( Scenes 8 - ) */

    public void LoadCarSelectScreenHoops()
    {
        SceneManager.LoadScene(8);
    }

    public void LoadHoopsLoadingScreen()
    {
        SceneManager.LoadScene(9);
    }

    public void LoadHoopsGame()
    {
        SceneManager.LoadScene(10);
    }

    /* --------- SHARED ---------- */

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator SplashToStart()
    {
        // Stuff to happen before
        yield return new WaitForSeconds(timeToWait);
        
        // Stuff to happen after
        LoadStartScreen();
    }

}
