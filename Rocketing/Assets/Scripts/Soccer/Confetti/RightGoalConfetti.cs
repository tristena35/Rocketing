using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightGoalConfetti : MonoBehaviour
{
    private ParticleSystem confetti;

    // Start is called before the first frame update
    void Start()
    {
       confetti = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartConfetti()
    {
        confetti.Play();
    }
}