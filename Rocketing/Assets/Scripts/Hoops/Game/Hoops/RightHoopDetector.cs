using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHoopDetector : MonoBehaviour
{
    //AudioClips
    [SerializeField] AudioClip[] goalSFXs;

    // Volumes
    float goalSoundVolume = 0.9f;

    // Time Values
    [SerializeField] int disablingTime = 2;

    // Cached References
    HoopsGame hoopsGame;
    HoopBall ball;
    ScoreKeeper scoreKeeper;
    LeftNetDetector leftNetDetector;
    RightGoalConfetti rightConfetti;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate Game Object References
        hoopsGame = FindObjectOfType<HoopsGame>();
        ball = FindObjectOfType<HoopBall>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        leftNetDetector = FindObjectOfType<LeftNetDetector>();
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
        gameObject.GetComponent<BoxCollider2D>().enabled = false; 
        Debug.Log("Disabled Hoop Trigger");

        yield return new WaitForSeconds(disablingTime);
        
        // Enables Trigger
        gameObject.GetComponent<BoxCollider2D>().enabled = true; 
        Debug.Log("Enabled Hoop Trigger");
    }

    // Only PLAYER ONE can score on this net
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.CompareTag("GameBall") )
        {
            //ball.ExplodeBall();
            // Randomize ball sound
            AudioClip goalSFX = goalSFXs[ UnityEngine.Random.Range( 0, goalSFXs.Length ) ];
            // Show Confetti
            rightConfetti.StartConfetti();
            // Play sound effect
            AudioSource.PlayClipAtPoint(goalSFX, 
                                        Camera.main.transform.position, 
                                        goalSoundVolume);
            scoreKeeper.AddLeftScore();
            hoopsGame.PlayerScored("One");
        }
    }
}
