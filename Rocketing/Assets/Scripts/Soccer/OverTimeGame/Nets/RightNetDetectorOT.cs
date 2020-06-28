using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightNetDetectorOT : MonoBehaviour
{
    //AudioClips
    [SerializeField] AudioClip[] goalSFXs;

    // Volumes
    float goalSoundVolume = 0.9f;

    // Time Values
    int disablingTime = 3;

    // Cached References
    OvertimeGame overtimeGame;
    OverTimeCanvas OTCanvas;
    ScoreKeeper scoreKeeper;
    LeftNetDetectorOT leftNetDetector;
    RightGoalConfetti rightConfetti;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate Game Object References
        overtimeGame = FindObjectOfType<OvertimeGame>();
        OTCanvas = FindObjectOfType<OverTimeCanvas>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        leftNetDetector = FindObjectOfType<LeftNetDetectorOT>();
        rightConfetti = FindObjectOfType<RightGoalConfetti>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableGoalTrigger()
    {
        StartCoroutine( DisableTriggerAfterGoal() );
    }

    IEnumerator DisableTriggerAfterGoal()
    {
        // Disables Trigger
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false; 
        Debug.Log("Disabled Right Goal Trigger");

        yield return new WaitForSeconds(disablingTime);
        
        // Enables Trigger
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true; 
        Debug.Log("Enabled Right Goal Trigger");
    }

    // Only PLAYER ONE can score on this net
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.CompareTag("GameBall") )
        {
            // Randomize ball sound
            AudioClip goalSFX = goalSFXs[ UnityEngine.Random.Range( 0, goalSFXs.Length ) ];
             // Show Confetti
            rightConfetti.StartConfetti();
            // Play sound effect
            AudioSource.PlayClipAtPoint(goalSFX, 
                                        Camera.main.transform.position, 
                                        goalSoundVolume);
            scoreKeeper.AddLeftScore();
            OTCanvas.ChangeLeftCanvasScore();
            overtimeGame.PlayerScored("One");
        }
    }
}
