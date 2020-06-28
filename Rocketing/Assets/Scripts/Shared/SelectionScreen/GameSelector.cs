using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameSelector : MonoBehaviour
{
    // Cached Referencez
    LeftCarSelect leftCar;
    RightCarSelect rightCar;
    MapSelect mapSelector;
    TimeSelect timeSelector;

    // Sprites for game that players chose
    [SerializeField] public Sprite chosenLeftCar;
    [SerializeField] public Sprite chosenRightCar;
    [SerializeField] public Sprite mapSelected;
    [SerializeField] public float timeSelected;
 
    // Build Scene Index
    int currentSceneIndex;
    
    private void Awake()
    {
        SetUpSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        leftCar = FindObjectOfType<LeftCarSelect>();
        rightCar = FindObjectOfType<RightCarSelect>();
        mapSelector = FindObjectOfType<MapSelect>();
        timeSelector = FindObjectOfType<TimeSelect>();
    }

    // Update is called once per frame
    void Update()
    {
        // Grabs the number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if( currentSceneIndex == 1 ) // Main Menu Screen Delete Object
        {
            Destroy(gameObject);
        }
    }

    /* ----- Arrow Buttons ----- */

    public void ClearAllFields()
    {
        chosenLeftCar = null;
        chosenRightCar = null;
        mapSelected = null;
    }

    // ----- CARS ----- //

    /* LEFT 

    public void LeftCarLeftButton()
    {
        Debug.Log("Left Left");
        leftCar.GoLeft();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public void LeftCarRightButton()
    {
        Debug.Log("Left Right");
        leftCar.GoRight();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }
    */

    /* RIGHT

    public void RightCarLeftButton()
    {
        Debug.Log("Right Left");
        rightCar.GoLeft();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public void RightCarRightButton()
    {
        Debug.Log("Right Right");
        rightCar.GoRight();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    */

    /* ----- MAPS ----- //

    public void LeftMapButton()
    {
        Debug.Log("Map Left");
        mapSelector.GoLeft();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }

    public void RightMapButton()
    {
        Debug.Log("Map Right");
        mapSelector.GoRight();
        // Play Click SFX
        AudioSource.PlayClipAtPoint(buttonClickNoise, 
                                Camera.main.transform.position, 
                                buttonClickVolume);
    }
    */

    /* --- Car, Map, Time GETTERS() --- */

    public void GetLeftCar() // Right before scene changes, grab their car
    {
        chosenLeftCar = leftCar.GetCarSprite();
    }
    
    public void GetRightCar() // Right before scene changes, grab their car
    {
        chosenRightCar = rightCar.GetCarSprite();
    }

    public void GetGameMap() // Right before scene changes, grab the map
    {
        mapSelected = mapSelector.GetMapSprite();
    }

    public void GetGameTime() // Right before scene changes, grab the time
    {
        timeSelected = timeSelector.GetTimeSelected();
    }

    public void GetAllFields()
    {
        GetLeftCar();
        GetRightCar();
        GetGameMap();
        GetGameTime();
    }

    public Sprite GetGameCarLeft()
    {
        return chosenLeftCar;
    }

    public Sprite GetGameCarRight()
    {
        return chosenRightCar;
    }

    public Sprite GetMap()
    {
        return mapSelected;
    }

    public float GetTime()
    {
        return timeSelected;
    }

    private void SetUpSingleton()
    {
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
