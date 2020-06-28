using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    // AudioClips
    [SerializeField] AudioClip buttonClickNoise;

    // AudioClip Volume
    [SerializeField] [Range(0,1)] float buttonClickVolume = 0.4f;

    // Image
    [SerializeField] private Image chosenMap;

    // All Maps Array
    [SerializeField] public Sprite[] allMaps;

    // Index Of Original Red Car
    int currentMapIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        chosenMap = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoLeft()
    {
        currentMapIndex -- ;
        if (currentMapIndex < 0)
        {
            currentMapIndex = allMaps.Length - 1;
        }
        chosenMap.sprite = allMaps[ currentMapIndex ];
    }

    public void GoRight()
    {
        currentMapIndex ++ ;
        if (currentMapIndex > allMaps.Length - 1)
        {
            currentMapIndex = 0;
        }
        chosenMap.sprite = allMaps[ currentMapIndex ];
    }
    
    // ----- MAP BUTTONS ----- //

    public void LeftMapButton()
    {
        Debug.Log("Map Left");
        GoLeft();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public void RightMapButton()
    {
        Debug.Log("Map Right");
        GoRight();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public Sprite GetMapSprite()
    {
        return chosenMap.sprite;
    }
}
