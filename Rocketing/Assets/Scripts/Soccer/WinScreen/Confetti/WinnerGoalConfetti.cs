using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerGoalConfetti : MonoBehaviour
{
    [SerializeField] ParticleSystem winConfetti;

    // Start is called before the first frame update
    void Start()
    {
       //winConfetti = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartConfetti()
    {
        winConfetti.Play();
    }

    public void StopConfetti()
    {
        winConfetti.Stop();
    }
}
