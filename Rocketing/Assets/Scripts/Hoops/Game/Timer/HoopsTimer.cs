using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class HoopsTimer : MonoBehaviour
{
    // Cached References
    ScoreKeeper scoreKeeper;
    LeftHoopDetector leftHoopDetector;
    RightHoopDetector rightHoopDetector;
    HoopBall gameBall;
    GameSelector gameSelector;

    // Constant Number Values
    [SerializeField] [Range(10,120)] float timeRemaining = 180f;
    int timeTillTextHides = 2;

    // Booleans
    private bool gameIsDone = false;
    private bool timerIsRunning = false;
    private bool isTimerTextOn = true;
    private bool TenCountDownAudioShouldStart = true;
    private bool play60Seconds = true;
    private bool play30Seconds = true;
    [SerializeField] private bool gameIsOneMinuteLong = false;

    // Canvas Objects
    [SerializeField] TextMeshProUGUI gameTimerText;

    // AudioClips / AudioSources
    [SerializeField] AudioClip whistleSoundAudio;
    [SerializeField] AudioClip SixtySecondsLeftAudio;
    [SerializeField] AudioClip ThirtySecondsLeftAudio;
    private AudioSource TenSecondCountDownAudio;

    // Volumes
    [SerializeField] [Range(0,1)] float whistleVolume = 0.75f;
    [SerializeField] [Range(0,1)] float announcerVolume = 0.8f;

    private void Start()
    {
        // Instantiate Game Object References
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        leftHoopDetector = FindObjectOfType<LeftHoopDetector>();
        rightHoopDetector = FindObjectOfType<RightHoopDetector>();
        gameBall = FindObjectOfType<HoopBall>();
        gameSelector = FindObjectOfType<GameSelector>();

        // Get Time from selector
        timeRemaining = ( gameSelector.GetTime() * 60f ); // 60 seconds per minute

        // aSources is an array of all audiosource objects
        var aSources = GetComponents<AudioSource>();
        TenSecondCountDownAudio = aSources[1];

        // Set game timer text
        gameTimerText.text = gameSelector.GetTime().ToString() + ":00";

        if ( gameSelector.GetTime() == 1f ) // If at start, game is one minute long, turn true
        {
            gameIsOneMinuteLong = true;
        }
    }

    void Update()
    {
        if (timerIsRunning) // Timer Running
        {
            // If the timer is running, show text
            gameTimerText.enabled = isTimerTextOn;

            if (timeRemaining > 0.1) // Timer still running
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else // Timer Done
            {
                if ( gameBall.IsBallTouchingTheFloor() ) // If the game ball is touching floor
                {
                    DisplayTime("GAME");
                    timerIsRunning = false;
                    StartCoroutine( StartEndingGame() );
                }
                else // If ball is in the air
                {
                    DisplayTimeOne();
                }
                gameIsDone = true;
            }
            if (timeRemaining <= 60.00f && !gameIsDone && play60Seconds && !gameIsOneMinuteLong) // One minute left
            {
                // Play audioclip
                AudioSource.PlayClipAtPoint(SixtySecondsLeftAudio, 
                                    Camera.main.transform.position, 
                                    announcerVolume);
                Debug.Log("60 seconds");
                play60Seconds = false;

            }
            if (timeRemaining <= 30.00f && !gameIsDone && play30Seconds) // Half minute left
            {
                // Play audioclip
                AudioSource.PlayClipAtPoint(ThirtySecondsLeftAudio, 
                                    Camera.main.transform.position, 
                                    announcerVolume);
                Debug.Log("30 seconds");
                play30Seconds = false;

            }
            if (timeRemaining <= 10 && !gameIsDone) // Start Timer
            {
                if (TenCountDownAudioShouldStart) // If count down was started for first time
                {
                    Debug.Log("Started 10 Timer");
                    TenSecondCountDownAudio.Play();
                    TenCountDownAudioShouldStart = false;
                }
            }
        }
        else if (timerIsRunning = false && gameIsDone)  // Finish Game
        {
            if( scoreKeeper.ForceOverTime() ) // If they are tied at the end of the game, go OT
            {
                Debug.Log("Hoops OVERTIME");
                GoToOverTimeGame();
            }
            else // If theres a winner, go to win screen
            {
                Debug.Log("Hoops ENDGAME");
                EndGame();
            }
        }
    }

    public void PauseGameTimer()
    {
        timerIsRunning = false;

        if(TenCountDownAudioShouldStart == false) // If 10 second count down has started already, thus should be false
        {
            TenSecondCountDownAudio.Pause();
        }
    }

    public void ResumeGameTimer()
    {
        timerIsRunning = true;
        
        if(TenCountDownAudioShouldStart == false && !gameIsDone) // If 10 second count down has started already, thus should be false
        {
            Debug.Log("ResumeTimer");
            TenSecondCountDownAudio.Play();
        }
    }

    public bool isTimerRunning()
    {
        return timerIsRunning;
    }

    public void TimeHasStarted()
    {
        timerIsRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        gameTimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    void DisplayTimeOne()
    {
        gameTimerText.text = "0:01";
    }

    void DisplayTime(string time)
    {
        gameTimerText.text = time;
    }
    
    // When GameTimer runs out
    void EndGame()
    {
        SceneManager.LoadScene(11); // Go to Winner Screen
    }

    void GoToOverTimeGame()
    {
        SceneManager.LoadScene(13); // Hoops OverTime Screen
    }

    IEnumerator StartEndingGame()
    {
        // No one can score once whistle is blown
        leftHoopDetector.DisableGoalTrigger();
        rightHoopDetector.DisableGoalTrigger();
        
        //Play Audio Clips
        AudioSource.PlayClipAtPoint(whistleSoundAudio, 
                                    Camera.main.transform.position, 
                                    whistleVolume);

        // Stuff to happen before
        yield return new WaitForSeconds(timeTillTextHides);
        
        // Disable text (Hide)
        gameTimerText.enabled = !isTimerTextOn;

        if( scoreKeeper.ForceOverTime() ) // If they are tied at the end of the game, go OT
        {
            Debug.Log("Hoops Overtime");
            GoToOverTimeGame();
        }
        else
        {
            Debug.Log("Hoops Endgame");
            // Go to end game screen
            EndGame();
        }
    }
}
