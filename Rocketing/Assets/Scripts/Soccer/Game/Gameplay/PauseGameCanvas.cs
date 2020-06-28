using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ShowPauseMenu()
    {
        gameObject.SetActive(true);
    }

    public void HidePauseMenu()
    {
        gameObject.SetActive(false);
    }
}
