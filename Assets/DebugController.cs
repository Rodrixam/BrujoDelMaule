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
        if (inputTracker.GetInputDown(ControllerButton.aButton))
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;

            if(index >= SceneManager.sceneCount){ index = 0; }

            SceneManager.LoadScene(index);
        }

        //Reload current scene
        if (inputTracker.GetInputDown(ControllerButton.bButton))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
