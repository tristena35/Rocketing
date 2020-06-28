using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Constant Number Values
    float timeRemaining = 3f;
    int timeTillTextHides = 1;

    // Booleans
    private bool timerIsRunning = false;
    private bool isTimerTextOn = true;

    // Canvas Objects
    [SerializeField] TextMeshProUGUI timeText;

    private void Start()
    {
        
    }

    void Update()
    {
        if (timerIsRunning)
        {
            
            // If the timer is running, show text
            timeText.enabled = isTimerTextOn;

            if (timeRemaining > 0) // Timer still running
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else // Timer Done
            {
                DisplayTime("GO!");
                timerIsRunning = false;
                timeRemaining = 3f;
                StartCoroutine( HideCountDown() );
            }
        }
    }

    IEnumerator HideCountDown()
    {
        // Stuff to happen before
        yield return new WaitForSeconds(timeTillTextHides);
        
        // Disable text (Hide)
        timeText.enabled = !isTimerTextOn;
    }

    public void TimeHasStarted()
    {
        timerIsRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = seconds.ToString();
    }

    void DisplayTime(string time)
    {
        timeText.text = time;
    }

}