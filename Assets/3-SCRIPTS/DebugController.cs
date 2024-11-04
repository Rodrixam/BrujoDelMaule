using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    InputTracker inputTracker;

    private void Awake()
    {
        GameObject aux = GameObject.Find("DebugController");
        if (aux != null && aux != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        inputTracker = GetComponent<InputTracker>();
    }

    void Update()
    {
        //Load next scene
        if (inputTracker.GetInputDown(ControllerButton.xButton))
        {
            Debug.Log("Current scene index " + SceneManager.GetActiveScene().buildIndex + " - Scene count " + SceneManager.sceneCountInBuildSettings);
            int index = SceneManager.GetActiveScene().buildIndex + 1;

            if(index >= SceneManager.sceneCountInBuildSettings) { index = 0; }

            Debug.Log("Loading next scene " + index);
            SceneManager.LoadScene(index);
        }

        //Reload current scene
        if (inputTracker.GetInputDown(ControllerButton.yButton))
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            Debug.Log("Reloading scene " + index);
            SceneManager.LoadScene(index);
        }
    }
}
