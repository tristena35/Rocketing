using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftNetDetectorOT : MonoBehaviour
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
    RightNetDetectorOT rightNetDetector;
    LeftGoalConfetti leftConfetti;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate Game Object References
        overtimeGame = FindObjectOfType<OvertimeGame>();
        OTCanvas = FindObjectOfType<OverTimeCanvas>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        rightNetDetector = FindObjectOfType<RightNetDetectorOT>();
        leftConfetti = FindObjectOfType<LeftGoalConfetti>();
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
        Debug.Log("Disabled Left Goal Trigger");

        yield return new WaitForSeconds(disablingTime);
        
        // Enables Trigger
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true; 
        Debug.Log("Enabled Left Goal Trigger");
    }

    // Only PLAYER TWO can score on this net
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.CompareTag("GameBall") )
        {
            // Randomize ball sound
            AudioClip goalSFX = goalSFXs[ UnityEngine.Random.Range( 0, goalSFXs.Length ) ];
            // Show Confetti
            leftConfetti.StartConfetti();
            // Play sound effect
            AudioSource.PlayClipAtPoint(goalSFX, 
                                        Camera.main.transform.position, 
                                        goalSoundVolume);
            scoreKeeper.AddRightScore();
            OTCanvas.ChangeRightCanvasScore();
            overtimeGame.PlayerScored("Two");
        }
    }
}
