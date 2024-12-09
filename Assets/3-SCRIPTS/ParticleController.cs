using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    List<ParticleSystem> systems;

    public void Play()
    {
        foreach(ParticleSystem ps in systems)
        {
            ps.Play();
        }
    }

    public void Stop()
    {
        foreach (ParticleSystem ps in systems)
        {
            ps.Stop();
        }
    }
}
