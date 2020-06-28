using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenSounds : MonoBehaviour
{

    // AudioClips
    [SerializeField] AudioClip carEngineRevAudio;
    // Volume
    [SerializeField] [Range(0,1)] float carRevSoundEffectVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(carEngineRevAudio, 
                                    Camera.main.transform.position, 
                                    carRevSoundEffectVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
