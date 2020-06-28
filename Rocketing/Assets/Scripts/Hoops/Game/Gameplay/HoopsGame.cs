using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HoopsGame : MonoBehaviour
{
    // Number Values
    [SerializeField] int countDownTime = 3;
    [SerializeField] int timeBeforeCountDown = 3;
    [SerializeField] int displayGoalTextTime = 1;
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

    // Cached References
    HoopBall hoopBall;
    LeftHoopCar leftCar;
    RightHoopCar rightCar;
    LeftHoopDetector leftHoopDetector;
    RightHoopDetector rightHoopDetector;
    LeftHoopSave leftSaveDetector;
    RightHoopSave rightSaveDetector;
    Timer timer;
    HoopsTimer gameTimer;
    PauseGameCanvas pauseCanvas;
    GameSelector gameSelector;

    // Canvas Objects
    [SerializeField] TextMeshProUGUI playerScoredText;

    // Volumes
    [SerializeField] [Range(0,1)] float inGameSoundEffectsVolume = 0.75f;
    [SerializeField] [Range(0,1)] float carRevSoundEffectVolume = 1f;

    // AudioClips / AudioSources
    [SerializeField] AudioClip pregameAudio;
    [SerializeField] AudioSource inGameMusic;
    [SerializeField] AudioClip announcerCountDownAudio;
    [SerializeField] AudioClip carEngineRevAudio;
    [SerializeField] AudioClip countDownSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        // Grabs the number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Instantiate Game Object References
        hoopBall = FindObjectOfType<HoopBall>();
        leftCar = FindObjectOfType<LeftHoopCar>();
        rightCar = FindObjectOfType<RightHoopCar>();
        leftHoopDetector = FindObjectOfType<LeftHoopDetector>();
        rightHoopDetector = FindObjectOfType<RightHoopDetector>();
        timer = FindObjectOfType<Timer>();
        gameTimer = FindObjectOfType<HoopsTimer>();
        pauseCanvas = FindObjectOfType<PauseGameCanvas>();
        gameSelector = FindObjectOfType<GameSelector>();
        rightSaveDetector = FindObjectOfType<RightHoopSave>();
        leftSaveDetector = FindObjectOfType<LeftHoopSave>();

        // Intialize Reference to audio source
        inGameMusic = GetComponent<AudioSource>();

        StartCoroutine( StartTheGame() );
    }

    // Update is called once per frame
    void Update()
    {
        HandlePausing();

        if( playerHasScored )
        {
            StartCoroutine( DisplayPlayerGoalText() );
        }
        if ( Input.GetKeyDown(KeyCode.Backspace) ) // Go to Next Screen
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

    }

    public void PlayerScored(string whichPlayerScored)
    {
        playerWhoScored = whichPlayerScored;
        playerHasScored = true;

        // Disable Goal Triggers temporarily
        leftHoopDetector.DisableGoalTrigger();
        rightHoopDetector.DisableGoalTrigger();
        rightSaveDetector.DisableSaveTrigger();
        leftSaveDetector.DisableSaveTrigger();

        // Pause Timer
        gameTimer.PauseGameTimer();
    }

    void ResetGameAfterGoal()
    {
        // Lock Game Objects
        hoopBall.StopGame();
        leftCar.StopGame();
        rightCar.StopGame(); 

        PlayCountDownSounds();

        StartCoroutine( CountDownTimer() );
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
        inGameMusic.Play();
    }

    void PauseInGameMusic()
    {
        inGameMusic.Pause();
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

        PlayInGameMusic();

        gameBeenStarted = true;
    }

    IEnumerator CountDownTimer()
    {
        timer.TimeHasStarted();

        yield return new WaitForSeconds(countDownTime);
        
        // UnLock Game Objects
        hoopBall.StartGame();
        leftCar.StartGame();
        rightCar.StartGame();       

        if( gameJustStarted )
        {
            // Start Game Timer
            gameTimer.TimeHasStarted();
            gameJustStarted = false;
        }

        // If the gameTimer was paused
        if( ! gameTimer.isTimerRunning() )
        {
            // Resume Timer
            gameTimer.ResumeGameTimer();
        }
    }

    IEnumerator DisplayPlayerGoalText()
    {

        // Get Player that scored, then enable text
        playerScoredText.text = "Player " + playerWhoScored + " Scored !";
        playerScoredText.enabled = true;
        playerHasScored = false;

        // Stuff to happen before
        yield return new WaitForSeconds(displayGoalTextTime);
        
        // Enable text (Hide)
        playerScoredText.enabled = false;

        // Reset Game Components to Start
        ResetGameAfterGoal();
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

    public void ClearSelection()
    {
        gameSelector.ClearAllFields();
    }
}
