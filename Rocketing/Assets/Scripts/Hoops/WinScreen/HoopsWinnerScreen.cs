using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoopsWinnerScreen : MonoBehaviour
{
    // Cached References
    ScoreKeeper scoreKeeper;
    LeftWinningGameCar leftWinCar;
    RightWinningGameCar rightWinCar;
    WinnerGoalConfetti winnerConfetti;

    // Strings
    string gameOutcome; // PlayerOne, Draw, PlayerTwo

    // Time
    int timeToWait;

    // Build Scene Index
    int currentSceneIndex;

    // AudioClips and AudioSources
    [SerializeField] AudioClip postgameDrawAudio;
    [SerializeField] AudioSource postgameAudio;

    // Volumes
    float audioClipVolume = 0.65f;

    private void Awake()
    {
        SetUpSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize References
        scoreKeeper = FindObjectOfType<ScoreKeeper> ();
        leftWinCar = FindObjectOfType<LeftWinningGameCar> ();
        rightWinCar = FindObjectOfType<RightWinningGameCar> ();
        winnerConfetti = FindObjectOfType<WinnerGoalConfetti> ();

        // Intialize Reference to audio source
        postgameAudio = GetComponent<AudioSource> ();
        //postgameDrawAudio = GetComponent<AudioSource> ();
                                
        EvaluateWinnerDisplay();

        // Load End Scene
        StartCoroutine( WinToEndScreen() );
    }

    // Update is called once per frame
    void Update()
    {
        // Grabs the number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if( currentSceneIndex == 1 || currentSceneIndex == 10 ) // If you go to start or game, delete
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

    void EvaluateWinnerDisplay() // Left, Draw, Right
    {
        gameOutcome = scoreKeeper.GetGameOutcome();
        if( gameOutcome == "Player One" ) // Show just LEFT car
        {
            timeToWait = 10;
            rightWinCar.Hide();
            postgameAudio.Play();
            winnerConfetti.StartConfetti();
        }
        else if( gameOutcome == "Draw" )  // Show BOTH cars
        {
            timeToWait = 6;
            AudioSource.PlayClipAtPoint(postgameDrawAudio, 
                                    Camera.main.transform.position, 
                                    audioClipVolume);
        }
        else                              // Show just RIGHT car
        {
            timeToWait = 10;
            leftWinCar.Hide();
            postgameAudio.Play();
            winnerConfetti.StartConfetti();
        }
    }

    IEnumerator WinToEndScreen()
    {
        yield return new WaitForSeconds(timeToWait);
        
        // Stop Confetti
        winnerConfetti.StopConfetti();
        // Load last scene
        SceneManager.LoadScene(12);
    }
}
