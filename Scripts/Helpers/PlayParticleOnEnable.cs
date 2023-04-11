using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleOnEnable : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private void OnEnable()
    {
        if (particle != null) 
        { 
            particle.Play();
        }
    }
}
