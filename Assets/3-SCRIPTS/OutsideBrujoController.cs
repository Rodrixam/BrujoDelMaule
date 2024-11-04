using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideBrujoController : MonoBehaviour
{
    [SerializeField]
    float _killTime;

    [SerializeField]
    ObjectSpawnController _spawnController;

    [SerializeField]
    GameController _gameController;

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
        _gameController.GameOver();
    }

    public void TryCross()
    {
        _spawnController.summoned = false;
        gameObject.SetActive(false);
    }
}