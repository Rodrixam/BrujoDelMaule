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

    [SerializeField]
    JumpscareScript _jumpscareScript;

    public bool summoned = false;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Start()
    {
        StartCoroutine(TryToSpawn());
    }

    IEnumerator TryToSpawn()
    {
        yield return new WaitForSeconds(timeToTry);
        if (!summoned && Random.value <= spawnChance)
        {
            _jumpscareScript.tuetueActivated = true;
            summoned = true;
            _audioSource.Play();
        }

        StartCoroutine(nameof(TryToSpawn));
    }

    public void StopTueTue(bool deactivateJumpscare = true)
    {
        if (deactivateJumpscare)
        {
            _jumpscareScript.tuetueActivated = false;
        }
        _audioSource.Stop();
        summoned = false;

        StopAllCoroutines();
        StartCoroutine(TryToSpawn());
    }
}
