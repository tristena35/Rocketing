using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBackgroundManager : MonoBehaviour
{
    // Cached References
    GameSelector gameSelector;

    // Sprite Renderer Component
    Image backgroundImage;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate Cached Reference
        gameSelector = FindObjectOfType<GameSelector>();

        // Get rendered component
        backgroundImage = gameObject.GetComponent<Image>();    
        backgroundImage.sprite = gameSelector.GetMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
