using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSaveDetector : MonoBehaviour
{
    //AudioClips
    [SerializeField] AudioClip[] saveSFXs;

    // Volumes
    float saveSoundVolume = 0.9f;

    // Time Values
    [SerializeField] int disablingTime = 2;
    float saveTime = 0.5f;

    // Cached References
    GameTimer gameTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate Game Object References
        gameTimer = FindObjectOfType<GameTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableSaveTrigger()
    {
        StartCoroutine( DisableTheSaveTrigger() );
    }

    IEnumerator DisableTheSaveTrigger()
    {
        // Disables Trigger
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false; 
        Debug.Log("Disabled left Save Trigger");

        yield return new WaitForSeconds(disablingTime);
        
        // Enables Trigger
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true; 
        Debug.Log("Enabled left Save Trigger");
    }

    IEnumerator DidTheySave()
    {
        // Wait a second to see if timer wasnt stopped, meaning no goal
        yield return new WaitForSeconds(saveTime);
        
        if ( gameTimer.isTimerRunning() ) // If after a second, if timer is still running, they didn't score
        {
            // Randomize ball sound
            AudioClip saveSFX = saveSFXs[ UnityEngine.Random.Range( 0, saveSFXs.Length ) ];
            AudioSource.PlayClipAtPoint(saveSFX, 
                                        Camera.main.transform.position, 
                                        saveSoundVolume);
        }
    }

    // Only PLAYER TWO can score on this net
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.CompareTag("GameBall") )
        {
            StartCoroutine( DidTheySave() );
        }
    }
}
