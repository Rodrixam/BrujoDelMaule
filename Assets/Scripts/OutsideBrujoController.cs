using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideBrujoController : MonoBehaviour
{
    [SerializeField]
    float _killTime;

    [SerializeField]
    ObjectSpawnController _spawnController;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(KillRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator KillRoutine()
    {
        yield return new WaitForSeconds(_killTime);
    }

    public void TryCross()
    {
        gameObject.SetActive(false);
    }
}