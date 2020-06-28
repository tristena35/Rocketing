using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    // Build Scene Index
    int currentSceneIndex;

    // Canvas Objects
    [SerializeField] TextMeshProUGUI leftScoreText;
    [SerializeField] TextMeshProUGUI rightScoreText;

    // Score Values
    [SerializeField] int leftScore = 0;
    [SerializeField] int rightScore = 0;

    // Winner (Left, Right, Draw)
    string gameOutcome;

    private void Awake()
    {
        SetUpSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Grabs the number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if( currentSceneIndex == 1 || currentSceneIndex == 2 || currentSceneIndex == 6 || currentSceneIndex == 12) // End Screen Delete Object or Main Menu or Car Select
        {
            Destroy(gameObject);
        }

        CalculateWinner();
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

    public void AddLeftScore()
    {
        leftScore ++;
        leftScoreText.text = leftScore.ToString();
    }

    public void AddRightScore()
    {
        rightScore ++;
        rightScoreText.text = rightScore.ToString();
    }

    public int GetLeftScore()
    {
        return leftScore;
    }

    public int GetRightScore()
    {
        return rightScore;
    }

    void CalculateWinner()
    {
        if( leftScore > rightScore )
        {
           gameOutcome = "Player One";
        }
        else if( leftScore == rightScore )
        {
            gameOutcome = "Draw";
        }
        else
        {
            gameOutcome = "Player Two";
        }
    }

    public bool ForceOverTime()
    {
        return leftScore == rightScore; // Returns true if they are tied
    }

    public string GetGameOutcome()
    {
        return gameOutcome;
    }

}
