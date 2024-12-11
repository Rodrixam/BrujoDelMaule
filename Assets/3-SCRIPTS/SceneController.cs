using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    float time = 0;

    public void SetTime(float time)
    {
        this.time = time;
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneRoutine(index, time));
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneRoutine(scene, time));
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().buildIndex + 1, time));
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().buildIndex, time));
    }

    IEnumerator LoadSceneRoutine(int index, float time = 0)
    {
        if(time != 0)
        {
            yield return new WaitForSeconds(time);
        }

        SceneManager.LoadScene(index);
    }

    IEnumerator LoadSceneRoutine(string scene, float time = 0)
    {
        if (time != 0)
        {
            yield return new WaitForSeconds(time);
        }

        SceneManager.LoadScene(scene);
    }
}
