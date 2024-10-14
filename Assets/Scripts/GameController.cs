using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    SaltBagController _saltBag;

    [SerializeField]
    CrossController _crossMimic;

    [SerializeField]
    TueTueSpawnController _tueSpawn;

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
        SceneManager.LoadScene(0);
    }
}
