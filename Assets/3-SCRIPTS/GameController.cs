using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField]
    float _timeToFinish = 60;

    [SerializeField]
    UnityEvent winEvent;

    [SerializeField]
    UnityEvent loseEvent;

    [SerializeField]
    ClockController clock = null;

    [SerializeField]
    SaltBagController _saltBag;

    [SerializeField]
    CrossController _crossMimic;

    [SerializeField]
    TueTueSpawnController _tueSpawn;

    private void Start()
    {
        StartCoroutine(Win());

        if (clock != null)
        {
            clock.SetMaxTime(_timeToFinish);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_saltBag.canPullSalt || _tueSpawn.summoned)
        {
            _crossMimic.canMimic = false;
        }
        else
        {
            _crossMimic.canMimic = true;
        }
    }

    public void GameOver()
    {
        StopAllCoroutines();
        loseEvent.Invoke();
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(_timeToFinish);
        winEvent.Invoke();
    }
}
