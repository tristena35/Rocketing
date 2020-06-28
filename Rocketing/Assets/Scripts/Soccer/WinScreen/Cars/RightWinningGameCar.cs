using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWinningGameCar : MonoBehaviour
{
    // Sprite Renderer and Collider Component to get car chosen
    SpriteRenderer spriteRenderer;
    PolygonCollider2D polygonCollider;

    // Cached References
    GameSelector gameSelector;

    // Configuration Parameters
    [SerializeField] float xPadding = 0.75f;
    [SerializeField] float yPadding = 0.5f;
    float jumpForce = 6.0f;

    // RigidBody for Car
    public Rigidbody2D CarBody;

    // Boolean Values
    public bool isGrounded = true;
    private bool isUpsideDown = false;

    // Coordinate Variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Rotating variable
    int rotatingSpeed = 360;

    // Number of Jumps
    int numberOfJumps = 2;

    void Start()
    {
        // Sets up camera boundaries
        SetUpMoveBoundaries();

        // Instantiate Cached Reference
        gameSelector = FindObjectOfType<GameSelector>();

        // Get rendered component
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();    
        spriteRenderer.sprite = gameSelector.GetGameCarRight();

        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();

        // Instantiating References
        CarBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfCarIsUpsideDown();

        if( isUpsideDown ) // If the car is upside down
        {
            FlipCar();
        }

        Jump();
        Rotate();
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
        Vector2 upwardForce = new Vector2 (0, jumpForce );
        // If space bar was pushed, flip car
        if(Input.GetButtonDown("Jump2") && isGrounded)
        {
            CarBody.AddForce(upwardForce, ForceMode2D.Impulse);
            // When Jumping, you are no longer grounded
            isGrounded = false;
        }
    }

    void Rotate()
    {
        var rotation = Input.GetAxis("RotateRightCar") * Time.deltaTime * rotatingSpeed;

        // Update position of the car
        transform.Rotate(Vector3.forward * rotation);

        /*
        // When U is pressed -> Rotate Left
        if (Input.GetKey(KeyCode.U))
        {
            transform.Rotate(Vector3.forward * rotatingSpeed * Time.deltaTime);
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
            if(numberOfJumps == 1)
            {
                jumpForce = 4.5F;
                CarBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                numberOfJumps --;
            }
            else
            {
                CarBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                numberOfJumps --;
            }
            if (numberOfJumps == 0)
            {
                // When Jumping, you are no longer grounded
                isGrounded = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         // Reset so you can jump again
        isGrounded = true;
        jumpForce = 6f;
        numberOfJumps = 2;
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - xPadding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - yPadding;
    }

    public void Hide()
    {
        Destroy(gameObject);
    }
}
