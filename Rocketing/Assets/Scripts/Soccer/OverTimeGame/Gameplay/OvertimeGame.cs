using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OvertimeGame : MonoBehaviour
{
    // Number Values
    [SerializeField] int countDownTime = 3;
    [SerializeField] int timeBeforeCountDown = 3;
    int displayGoalTextTime = 3;
    int currentSceneIndex;

    // String Value for which player scored: 1 or 2
    string playerWhoScored = "";

    // Booleans
    private bool playerHasScored = false;
    private bool gameJustStarted = true;
    private bool gameBeenStarted = false;
    [SerializeField] private bool gameIsPaused = false;
    private bool allow = true;
    private bool allow2 = true;
    private bool overtimeStillGoing = true;

    // Cached References
    GameBall gameBall;
    LeftCarControls leftCar;
    RightCarControls rightCar;
    LeftNetDetectorOT leftNetDetector;
    RightNetDetectorOT rightNetDetector;
    LeftSaveDetectorOT leftSaveDetector;
    RightSaveDetectorOT rightSaveDetector;
    OvertimeTimer timer;
    PauseGameCanvas pauseCanvas;
    GameSelector gameSelector;
    PreserveCanvas preserveCanvas;

    // Canvas Objects
    [SerializeField] TextMeshProUGUI playerScoredText;

    // Volumes
    [SerializeField] [Range(0,1)] float inGameSoundEffectsVolume = 0.75f;
    [SerializeField] [Range(0,1)] float carRevSoundEffectVolume = 1f;

    // AudioClips / AudioSources
    [SerializeField] AudioClip pregameAudio;
    [SerializeField] AudioClip overtimeYellAudio;
    [SerializeField] AudioSource OvertimeGameMusic;
    [SerializeField] AudioClip announcerCountDownAudio;
    [SerializeField] AudioClip carEngineRevAudio;
    [SerializeField] AudioClip countDownSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        // Grabs the number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Instantiate Game Object References
        gameBall = FindObjectOfType<GameBall>();
        leftCar = FindObjectOfType<LeftCarControls>();
        rightCar = FindObjectOfType<RightCarControls>();
        leftNetDetector = FindObjectOfType<LeftNetDetectorOT>();
        rightNetDetector = FindObjectOfType<RightNetDetectorOT>();
        rightSaveDetector = FindObjectOfType<RightSaveDetectorOT>();
        leftSaveDetector = FindObjectOfType<LeftSaveDetectorOT>();
        timer = FindObjectOfType<OvertimeTimer>();
        pauseCanvas = FindObjectOfType<PauseGameCanvas>();
        gameSelector = FindObjectOfType<GameSelector>();
        preserveCanvas = FindObjectOfType<PreserveCanvas>();

        // Intialize Reference to audio source
        OvertimeGameMusic = GetComponent<AudioSource>();

        // At the start of the scene queue the overtime yell audioclip
        AudioSource.PlayClipAtPoint(overtimeYellAudio, 
                                    Camera.main.transform.position, 
                                    inGameSoundEffectsVolume); 

        StartCoroutine( StartTheGame() );
    }

    // Update is called once per frame
    void Update()
    {
        HandlePausing();

        if( playerHasScored ) // Then End The Game
        {
            overtimeStillGoing = false; // If someone scored, overtime is done
            StartCoroutine( DisplayOverTimeWinnerText() );
        }
        if ( Input.GetKeyDown(KeyCode.Backspace) ) // Go to Next Screen
        {
            SceneManager.LoadScene(5); // Win Screen
        }

    }

    public void PlayerScored(string whichPlayerScored)
    {
        playerWhoScored = whichPlayerScored;
        playerHasScored = true;

        // Disable Goal Triggers temporarily
        leftNetDetector.DisableGoalTrigger();
        rightNetDetector.DisableGoalTrigger();
        rightSaveDetector.DisableSaveTrigger();
        leftSaveDetector.DisableSaveTrigger();
    }

    void PlayCountDownSounds()
    {
        AudioSource.PlayClipAtPoint(announcerCountDownAudio, 
                                    Camera.main.transform.position, 
                                    inGameSoundEffectsVolume); 
        AudioSource.PlayClipAtPoint(countDownSoundEffect, 
                                    Camera.main.transform.position, 
                                    inGameSoundEffectsVolume);     
    }

    void PlayInGameMusic()
    {
        OvertimeGameMusic.Play();
    }

    void PauseInGameMusic()
    {
        OvertimeGameMusic.Pause();
    }

    public bool IsOvertimeStillGoing()
    {
        return overtimeStillGoing;
    }

    IEnumerator StartTheGame()
    {
        //Play Audio Clips
        AudioSource.PlayClipAtPoint(pregameAudio, 
                                    Camera.main.transform.position, 
                                    inGameSoundEffectsVolume);
        AudioSource.PlayClipAtPoint(carEngineRevAudio, 
                                    Camera.main.transform.position, 
                                    carRevSoundEffectVolume);

        yield return new WaitForSeconds(timeBeforeCountDown);
        
        PlayCountDownSounds();

        StartCoroutine( CountDownTimer() );

        gameBeenStarted = true;
    }

    IEnumerator CountDownTimer()
    {
        timer.TimeHasStarted();

        yield return new WaitForSeconds(countDownTime);
        
        // UnLock Game Objects
        gameBall.StartGame();
        leftCar.StartGame();
        rightCar.StartGame();    

        // Raise Volume   
        OvertimeGameMusic.volume = 0.75f;
    }

    IEnumerator DisplayOverTimeWinnerText()
    {
        // Get Player that scored, then enable text
        playerScoredText.text = "Player " + playerWhoScored + " Scored !";
        playerScoredText.enabled = true;
        playerHasScored = false;

        // Stuff to happen before
        yield return new WaitForSeconds(displayGoalTextTime);
        
        // Enable text (Hide)
        playerScoredText.enabled = false;

        SceneManager.LoadScene(5); // Load Win Screen
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseCanvas.ShowPauseMenu();
        if( gameBeenStarted && allow)
        {
            PauseInGameMusic();
            allow = false;
            allow2 = true;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseCanvas.HidePauseMenu();
        if( gameBeenStarted && allow2 )
        {
            PlayInGameMusic();
            allow2 = false;
            allow = true;
        }
        gameIsPaused = false;
    }

    private void HandlePausing()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) ) // Player Pauses Game
        {
            gameIsPaused = !gameIsPaused;
        }
        if( gameIsPaused )  // Paused
        {
            PauseGame();
        }
        if( !gameIsPaused ) // Not paused
        {
            ResumeGame();
        }
    }

    public void NormalizeGameTime()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        Debug.Log("Here");
    }
}
