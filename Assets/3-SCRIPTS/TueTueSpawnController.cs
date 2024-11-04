using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TueTueSpawnController : MonoBehaviour
{
    [SerializeField]
    float timeToTry = 10f;

    [SerializeField]
    [Range(0, 1)]
    float spawnChance = 0.5f;

    [SerializeField]
    AudioSource _audioSource;

    public bool summoned = false;

    void Start()
    {
        Invoke(nameof(TryToSpawn), timeToTry);
    }

    bool TryToSpawn()
    {
        if (!summoned && Random.value <= spawnChance)
        {
            summoned = true;
            _audioSource.Play();
        }

        Invoke(nameof(TryToSpawn), timeToTry);
        return false;
    }

    public void StopTueTue()
    {
        _audioSource.Stop();
        summoned = false;
    }
}
