using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCarSelect : MonoBehaviour
{
    // AudioClips
    [SerializeField] AudioClip buttonClickNoise;

    // AudioClip Volume
    [SerializeField] [Range(0,1)] float buttonClickVolume = 0.4f;
    
    // Sprite Renderer Component
    SpriteRenderer spriteRenderer;

    // Index Of Original Red Car
    [SerializeField] int currentCarIndex;

    // All Cars
    [SerializeField] public Sprite[] allCarsRight;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        currentCarIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoLeft()
    {
        currentCarIndex -- ;
        if (currentCarIndex < 0)
        {
            currentCarIndex = allCarsRight.Length - 1;
        }
        spriteRenderer.sprite = allCarsRight[ currentCarIndex ];
    }

    public void GoRight()
    {
        currentCarIndex ++ ;
        if (currentCarIndex > allCarsRight.Length - 1)
        {
            currentCarIndex = 0;
        }
        spriteRenderer.sprite = allCarsRight[ currentCarIndex ];
    }

    /* RIGHT BUTTONS */

    public void RightCarLeftButton()
    {
        Debug.Log("Right Left");
        GoLeft();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public void RightCarRightButton()
    {
        Debug.Log("Right Right");
        GoRight();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public Sprite GetCarSprite()
    {
        return spriteRenderer.sprite;
    }
}
