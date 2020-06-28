using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSelect : MonoBehaviour
{
    // AudioClips
    [SerializeField] AudioClip buttonClickNoise;

    // AudioClip Volume
    [SerializeField] [Range(0,1)] float buttonClickVolume = 0.4f;

    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] float startingTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoLeft()
    {
        startingTime -- ;
        if (startingTime < 1)
        {
            startingTime = 3f;
        }
        timeText.text = (startingTime).ToString() + ":00";
    }

    public void GoRight()
    {
        startingTime ++ ;
        if (startingTime > 3f)
        {
            startingTime = 1;
        }
        timeText.text = (startingTime).ToString() + ":00";
    }
    
    // ----- TIME BUTTONS ----- //

    public void LeftTimeButton()
    {
        Debug.Log("Time Left");
        GoLeft();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public void RightTimeButton()
    {
        Debug.Log("Time Right");
        GoRight();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public float GetTimeSelected()
    {
        return startingTime;
    }
}
