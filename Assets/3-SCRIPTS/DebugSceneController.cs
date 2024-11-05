using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneController : MonoBehaviour
{
    InputTracker inputTracker;

    private void Start()
    {
        inputTracker =  FindAnyObjectByType<InputTracker>();
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
