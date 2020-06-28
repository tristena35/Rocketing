using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall : MonoBehaviour
{
    // Configuration Paramaters
    [SerializeField] private float kickForce = 5f;

    // AudioClips
    [SerializeField] AudioClip kickBallSFX;

    // Volumes
    [SerializeField] [Range(0,1)] float kickBallSFXVolume = 0.5f;

    // Cached Component References
    Rigidbody2D gameBallRigidBody;

    // Start Position Of Game Ball
    Vector3 startPos;

    // Boolean variable to check if the game has started
    bool hasStarted = false;
    bool rollingLeft = false; // Ball moving left, rotate left
    bool rollingRight = false; // Ball moving right, rotate right
    bool isTouchingTheFloor = false; // Ball starts in air, so not touching floor -> false

    // Start is called before the first frame update
    void Start()
    {
        // RigidBody
        gameBallRigidBody = GetComponent<Rigidbody2D>();

        // Get Original Position of ball
        startPos = new Vector3(transform.position.x, transform.position.y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if( ! hasStarted ) // If Game has NOT started
        {
            LockBall();
        }
        else                // If Game HAS started
        {
            GetDirectionOfBall();
            // Debug.Log("Rolling Left: " + rollingLeft + "     Rolling Right: " + rollingRight);
            if( rollingLeft )
                RotateBallLeft();
            else if( rollingRight )
                RotateBallRight();
        }
    }

    void GetDirectionOfBall()
    {
        float xVelocity = gameObject.GetComponent<Rigidbody2D>().velocity.x;

        if( xVelocity > 0 ) // Going to the right
        {
            rollingRight = true;
            rollingLeft = false;
        }
        else // Going to the left
        {
            rollingLeft = true;
            rollingRight = false;
        }
    }

    void RotateBallLeft()
    {
        float xVelocity = Mathf.Abs( ( gameObject.GetComponent<Rigidbody2D>().velocity.x / 3 ) );
        transform.Rotate( Vector3.forward * xVelocity );
    }

    void RotateBallRight()
    {
        float xVelocity = Mathf.Abs( ( gameObject.GetComponent<Rigidbody2D>().velocity.x / 3 ) );
        transform.Rotate( Vector3.forward * -xVelocity );
    }

    public void StartGame()
    {
        hasStarted = true;
    }

    public void StopGame()
    {
        hasStarted = false;
    }

    public void LockBall()
    {
        // Start an lock ball at beginning position
        transform.position = startPos;
        // Set the velocity to 0 here so that the ball does not go flying at the start of the game
        gameBallRigidBody.velocity = new Vector3(0, 0, 0);
    }

    public bool IsBallTouchingTheFloor()
    {
        return isTouchingTheFloor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "Car" )
        {
            // Play Kick SFX
            AudioSource.PlayClipAtPoint(kickBallSFX, 
                                    Camera.main.transform.position, 
                                    kickBallSFXVolume); 
            // Calculate Direction of ball
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            gameBallRigidBody.AddForce(-direction * kickForce, ForceMode2D.Impulse);
            isTouchingTheFloor = false;
        }
        if ( collision.gameObject.tag == "GameFloor" ) // If Ball touches the floor
        {
            isTouchingTheFloor = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingTheFloor = false; // If it is no longer colliding with anything, set touching floor to false
    }

}
