using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class JumpscareScript : MonoBehaviour
{
    public bool wolfJumpscare = false;
    public bool tuetueActivated = false;

    InputTracker inputTracker;
    SceneController sceneController;
    bool over = false;

    [SerializeField]
    List<Light> lights = new List<Light>();

    [SerializeField]
    FadeController fade;

    [SerializeField]
    TeleportationAnchor chamberTeleport;

    [SerializeField]
    Animator brujoJumpscareAnimator;
    [SerializeField]
    AudioSource brujoJumpscareAudio;

    [SerializeField]
    Animator wolfJumpscareAnimator;
    [SerializeField]
    AudioSource wolfJumpscareAudio;

    [SerializeField]
    GameObject gameOverPanel;
    [SerializeField]
    TextMeshProUGUI gameOverTip;

    private void Awake()
    {
        inputTracker = FindAnyObjectByType<InputTracker>();
        sceneController = FindAnyObjectByType<SceneController>();
    }

    private void Update()
    {
        if (over)
        {
            if (inputTracker.GetInputDown(ControllerButton.aButton))
            {
                fade.FadeIn();
                sceneController.SetTime(2);
                sceneController.ReloadCurrentScene();
            }

            if (inputTracker.GetInputDown(ControllerButton.bButton))
            {
                fade.FadeIn();
                sceneController.SetTime(2);
                sceneController.LoadScene(0);
            }
        }
    }

    public void StartJumpscare()
    {
        StartCoroutine(TurnLightsOff());
    }

    IEnumerator TurnLightsOff()
    {
        bool done = false;

        while (!done)
        {
            done = true;
            foreach (Light light in lights)
            {
                if (light.intensity > 0)
                {
                    done = false;
                    light.intensity -= 0.5f * Time.deltaTime;
                }
            }
            yield return 0;
        }

        InitiateFade();
    }

    void InitiateFade()
    {
        fade.FadeIn();
        Invoke("Teleport", 2);
    }

    void Teleport()
    {
        chamberTeleport.RequestTeleport();
        fade.FadeOut();
        Invoke("JumpscareAnimation", 2);
    }

    void JumpscareAnimation()
    {
        if (wolfJumpscare)
        {
            wolfJumpscareAnimator.Play("jumpscare");
            wolfJumpscareAudio.Play();
        }
        else
        {
            brujoJumpscareAnimator.Play("jumpscare");
            brujoJumpscareAudio.Play();
        }
        Invoke("GameOverMessage", 1f);
    }

    void GameOverMessage()
    {
        gameOverPanel.SetActive(true);

        if (tuetueActivated)
        {
            gameOverTip.text = "El tuetue impedirá que puedas expulsar al bujo o al lobo, echa sal a la chimenea para callarlo";
        }
        else
        {
            if (wolfJumpscare)
            {
                gameOverTip.text = "Revisa el dormitorio para asegurarte que el lobo no está, puedes expulsarlo de la misma forma que al brujo";
            }
            else
            {
                gameOverTip.text = "Persinate mirando al brujo para expulsarlo, asegurate que este se encuentre en el centro de la cruz";
            }
        }

        over = true;
    }


}
