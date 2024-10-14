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

    // Update is called once per frame
    void Update()
    {
        _crossMimic.canMimic = !_saltBag.canPullSalt;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}
