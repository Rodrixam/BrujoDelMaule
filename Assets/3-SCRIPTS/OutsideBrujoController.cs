using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideBrujoController : MonoBehaviour
{
    [SerializeField]
    List<Vector3> _possiblePositions;

    [SerializeField]
    float _killTime;

    [SerializeField]
    ObjectSpawnController _spawnController;

    [SerializeField]
    GameController _gameController;

    [SerializeField]
    JumpscareScript _jumpscareScript;
    [SerializeField]
    bool wolf = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = _possiblePositions[Random.Range(0, _possiblePositions.Count)];
        StartCoroutine(KillRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator KillRoutine()
    {
        yield return new WaitForSeconds(_killTime);
        if (wolf)
        {
            _jumpscareScript.wolfJumpscare = true;
        }
        _gameController.GameOver();
    }

    public void TryCross()
    {
        _spawnController.summoned = false;
        gameObject.SetActive(false);
    }
}