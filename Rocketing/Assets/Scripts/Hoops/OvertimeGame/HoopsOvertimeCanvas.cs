using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HoopsOvertimeCanvas : MonoBehaviour
{
    // Cached References
    ScoreKeeper scoreKeeper;

    // Canvas Objects
    [SerializeField] TextMeshProUGUI leftCarScoreText;
    [SerializeField] TextMeshProUGUI rightCarScoreText;
    [SerializeField] TextMeshProUGUI playerAnnoucementText;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI OTText;

    // Time
    [SerializeField] float timeBeforeDisplayWinner = 1f;

    // Strings
    string outcomeWinner;

    // Booleans
    private bool isLastScene = false;
    private bool doOnce = true;

    // Scene and Scene Number
    private Scene currentScene;
    private int buildIndex;

    private void Awake()
    {
        SetUpSingleton();
    }

    // Start is called before the first frame update
    void Start() 
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        leftCarScoreText.text = scoreKeeper.GetLeftScore().ToString();
        rightCarScoreText.text = scoreKeeper.GetRightScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Create a temporary reference to the current scene.
        currentScene = SceneManager.GetActiveScene();

        // Retrieve the index of the scene in the project's build settings.
        buildIndex = currentScene.buildIndex;

        if( buildIndex == 1 || buildIndex == 2 || buildIndex == 12 ) // End Screen Delete Object or Main menu or Car Select
        {
            Destroy(gameObject);
        }

        if ( buildIndex == 11 && doOnce ) // In Win Screen
        {
            isLastScene = true;
            if ( isLastScene ) // In Win Screen
            {
                DisableAndEnable(); // Will only be called once
            }
            doOnce = false;
        }
    }

    private void DisableAndEnable() // Function to disable and enable text
    {
        // Disable
        countDownText.enabled = false;
        playerAnnoucementText.enabled = false;
        OTText.enabled = false;

        // Enable
        leftCarScoreText.enabled = true;
        rightCarScoreText.enabled = true;

        // Set isLastScene false so doesnt repeat to do this
        isLastScene = false;

        StartCoroutine( ShowWinnerName() );
    }

    public void ChangeLeftCanvasScore()
    {
        leftCarScoreText.text = scoreKeeper.GetLeftScore().ToString();
    }

    public void ChangeRightCanvasScore()
    {
        rightCarScoreText.text = scoreKeeper.GetRightScore().ToString();
    }

    private void SetUpSingleton()
    {
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    IEnumerator ShowWinnerName()
    {
        outcomeWinner = scoreKeeper.GetGameOutcome();

        yield return new WaitForSeconds(timeBeforeDisplayWinner);

        if( outcomeWinner == "Player One" ) // Player One
            playerAnnoucementText.text = outcomeWinner + " Wins !";
        else if( outcomeWinner == "Draw" ) // Draw
            playerAnnoucementText.text = outcomeWinner;
        else                               // Player Two
            playerAnnoucementText.text = outcomeWinner + " Wins !";

        playerAnnoucementText.enabled = true;
    }

    public void ChangeFont()
    {
        countDownText.fontSize = 80;
        countDownText.color = Color.yellow;
    }
}
