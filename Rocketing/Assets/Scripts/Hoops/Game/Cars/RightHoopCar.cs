using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHoopCar : MonoBehaviour
{
    // Sprite Renderer and Collider Component to get car chosen
    SpriteRenderer spriteRenderer;
    PolygonCollider2D polygonCollider;

    // Cached References
    GameSelector gameSelector;

    //AudioClips
    [SerializeField] AudioClip[] squeakSFXs;

    // Volumes
    float squeakVolume = 0.15f;

    // Configuration Parameters
    [Range (0f, 10f)] [SerializeField] float driveSpeed = 5f;
    [SerializeField] float xPadding = 0.75f;
    [SerializeField] float yPadding = 0.5f;
    [SerializeField] float jumpForce = 6.0f;
    int carMass = 500;

    // Start Position Of LeftCar
    Vector3 startPos;

    // RigidBody for Car
    public Rigidbody2D CarBody;

    // Boolean Values
    public bool isGrounded = true;
    private bool hasStarted = false;
    private bool isUpsideDown = false;
    [SerializeField] private bool playSqueak = false;

    // Number of Jumps
    int numberOfJumps = 2;

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
        spriteRenderer.sprite = gameSelector.GetGameCarRight();

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
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * driveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        // Update position of the car
        transform.position = new Vector3(newXPos, transform.position.y, 1);
    }

    void Rotate()
    {
        var rotation = Input.GetAxis("RotateRightCar") * Time.deltaTime * rotatingSpeed;

        // Update position of the car
        transform.Rotate(Vector3.forward * rotation);
        
        /* ------- THIS WORKS ------- //
        // When U is pressed -> Rotate Left
        if (Input.GetKey(KeyCode.U))
        {
            transform.Rotate(Vector3.forward * rotatingSpeed * Time.deltaTime);
            // WORKS --> transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * rotatingSpeed, Space.World);
        }
        // When O is pressed -> Rotate Left
        if (Input.GetKey(KeyCode.O))
        {
            transform.Rotate(Vector3.forward * -rotatingSpeed * Time.deltaTime);
        }*/
    }

    void Jump()
    {
        // If space bar was pushed, jump
        if(Input.GetButtonDown("Jump2") && isGrounded && numberOfJumps > 0)
        {
            playSqueak = true; // Now when you hit the ground you will hear a squeak
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
        if(Input.GetButtonDown("Jump2") && isGrounded)
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
        
        if ( collision.gameObject.tag == "GameFloor" && playSqueak) // If car hit the floor
        {
            // Play random squeak SFX once
            AudioClip squeakSFX = squeakSFXs[ UnityEngine.Random.Range( 0, squeakSFXs.Length ) ];
            AudioSource.PlayClipAtPoint(squeakSFX, 
                                        Camera.main.transform.position, 
                                        squeakVolume);
            playSqueak = false;
        }
    }
}
