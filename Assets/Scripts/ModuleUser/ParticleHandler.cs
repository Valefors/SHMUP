using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> _particles = new List<ParticleSystem>();

    public void PlayParticle(int index)
    {
        _particles[index].Play();
    }
}
