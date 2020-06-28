using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCarControls : MonoBehaviour
{

    // Sprite Renderer Component to get car chosen
    SpriteRenderer spriteRenderer;
    PolygonCollider2D polygonCollider;

    // Cached References
    GameSelector gameSelector;

    // Transform Object
    private Vector3 boostSpawnPos;

    // Configuration Parameters
    [Range (0f, 10f)] [SerializeField] float driveSpeed = 5f;
    [SerializeField] float boostDriveSpeed = 7.0f;
    [SerializeField] float xPadding = 0.75f;
    [SerializeField] float yPadding = 0.5f;
    [SerializeField] float jumpForce = 6.0f;
    int carMass = 500;
    [SerializeField] float durationOfBoost = 1f;
    [SerializeField] float boostRechargeTime = 5f;

    // Game Objects
    [SerializeField] GameObject boostVFX;

    [SerializeField] [Range(0,1)] float boostSoundVolume = 0.25f;

    // AudioClips
    [SerializeField] AudioClip boostSFX;

    // Start Position Of LeftCar
    Vector3 startPos;

    // RigidBody for Car
    public Rigidbody2D CarBody;

    // Number of Jumps
    int numberOfJumps = 2;

    // Boolean Values
    public bool isGrounded = true;
    private bool hasStarted = false;
    private bool boostIsReady = true;
    private bool isUpsideDown = false;

    // Coordinate Variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Rotating variable
    int rotatingSpeed = 360;

    void Start()
    {
        // Get Original Position of ball
        startPos = new Vector3(transform.position.x, transform.position.y, 1);

        // Instantiate Cached Reference
        gameSelector = FindObjectOfType<GameSelector>();

        // Get rendered component
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();    
        spriteRenderer.sprite = gameSelector.GetGameCarLeft();

        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();

        // Sets up camera boundaries
        SetUpMoveBoundaries();

        CarBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfCarIsUpsideDown();

        if( isUpsideDown ) // If the car is upside down
        {
            FlipCar();
        }

        if( ! hasStarted ) // Game has not started
        {
            LockCar();
        }
        else // Game Started
        {
            MoveHorizontal();
            Rotate();
            Jump();
        }

        /*if ( boostIsReady ) // Check to see when they will activate boost
        {
            Boost();
        }*/
    }

    public void StartGame()
    {
        hasStarted = true;
    }

    public void StopGame()
    {
        hasStarted = false;
    }

    void LockCar()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        // Put ball back to original spot
        transform.position = startPos;
        // Set the velocity to 0 here so the car starts still again
        CarBody.velocity = new Vector3(0, 0, 0);
    }

    void MoveHorizontal()
    {
        // Change in X
        var deltaX = Input.GetAxis("Horizontal1") * Time.deltaTime * driveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        // Update position of the car
        transform.position = new Vector3(newXPos, transform.position.y, 1);
    }

    void Rotate()
    {
        var rotation = Input.GetAxis("RotateLeftCar") * Time.deltaTime * rotatingSpeed;

        // Update position of the car
        transform.Rotate(Vector3.forward * rotation);
    } 

    void Jump()
    {
        // If space bar was pushed, jump
        if(Input.GetButtonDown("Jump1") && isGrounded && numberOfJumps > 0)
        {
            if(numberOfJumps == 1)
            {
                jumpForce = 4.5F;
                CarBody.AddForce(transform.up * jumpForce * carMass, ForceMode2D.Impulse);
                numberOfJumps --;
            }
            else
            {
                CarBody.AddForce(transform.up * jumpForce * carMass, ForceMode2D.Impulse);
                numberOfJumps --;
            }
            if (numberOfJumps == 0)
            {
                // When Jumping, you are no longer grounded
                isGrounded = false;
            }
        }
    }

    void CheckIfCarIsUpsideDown()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            isUpsideDown = true;
        }
        else
        {
            isUpsideDown = false;
        }
    }
    
    void FlipCar()
    {
        Vector2 upwardForce = new Vector2 (0, (jumpForce * carMass));
       
        // If space bar was pushed, flip car
        if(Input.GetButtonDown("Jump1") && isGrounded)
        {
            CarBody.AddForce(upwardForce, ForceMode2D.Impulse);
            // When Jumping, you are no longer grounded
            isGrounded = false;
        }
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - xPadding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - yPadding;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset so you can jump again
        isGrounded = true;
        jumpForce = 6f;
        numberOfJumps = 2;

        if ( collision.gameObject.tag == "GameBall" ) // If car hit the ball
        {
            // Quickly set velocity to 0 so that car does not rotate uncontrollably
            CarBody.velocity = new Vector3(0, 0, 0);
        }
    }
























    // ------- BOOST ------- //

    /*void Boost()
    {
        if( Input.GetKey(KeyCode.C) )
        {
            StartCoroutine( ShowBoost() );
        }
    }

    void CreateBoostVFX()
    {
        boostSpawnPos = new Vector3( transform.position.x, transform.position.y, 1 );

        // Create boost when 'W'' is pressed
        GameObject boost = Instantiate( boostVFX, boostSpawnPos, new Quaternion(0, 0 , 1, 0) ) as GameObject;

        Debug.Log("Boost: " + boost.transform.position);

        Destroy(boost, durationOfBoost);

        //Play sound effect
        AudioSource.PlayClipAtPoint(boostSFX, 
                                    Camera.main.transform.position, 
                                    boostSoundVolume);
    }

    private void BoostControls()
    {
        Debug.Log("Boost Controls");

        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * boostDriveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * boostDriveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector3(newXPos, newYPos, 1);
    }

    IEnumerator ShowBoost()
    {
        // Boost is no longer ready
        boostIsReady = false;

        // Disables Gravity
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.3f;

        CreateBoostVFX();
        BoostControls();

        yield return new WaitForSeconds(boostRechargeTime);

        // Enables Gravity
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        // Boost is now ready
        boostIsReady = true;    
    } */

}
