using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class CinematicController : MonoBehaviour
{
    InputTracker inputTracker;

    int index = 0;

    [SerializeField]
    float textSpeed = 0.1f;

    [SerializeField]
    List<Animator> animators = new List<Animator>();

    [SerializeField]
    List<string> lines = new List<string>();

    [SerializeField]
    List<AudioEvent> audioEvents = new List<AudioEvent>();

    [SerializeField]
    List<RequirementEvent> requirements = new List<RequirementEvent>();

    [SerializeField]
    List<GeneralEvent> unityEvents = new List<GeneralEvent>();

    [SerializeField]
    TextMeshProUGUI subtitles;

    Coroutine writing = null;

    private void Awake()
    {
        inputTracker = GameObject.Find("DebugController").GetComponent<InputTracker>();
    }

    private void Start()
    {
        //Activate new requirements
        foreach (RequirementEvent re in requirements)
        {
            if (index == re.GetIndex() && re.DoesActivateImmediately())
            {
                re.Activate();
            }
        }

        NextStep();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputTracker.GetInputDown(ControllerButton.aButton))
        {
            if(writing != null)
            {
                SpeedUpWriting();
            }
            else
            {
                NextStep();
            }
        }
    }

    void NextStep()
    {
        //////////////// OPENING //////////////// 
        Debug.Log("Opening step " + index);
        //Interrupt new step until requirements are fulfilled
        foreach (RequirementEvent re in requirements)
        {
            if (index == re.GetIndex() && !re.IsEventFulfilled())
            {
                Debug.Log("Step " + index + " can't open");
                return;
            }
        }

        //////////////// DEVELOPMENT //////////////// 
        Debug.Log("Developing step " + index);

        //Animations
        foreach (Animator a in animators)
        {
            a.Play(index.ToString());
        }

        //Audio events
        foreach (AudioEvent ae in audioEvents)
        {
            if (index - 1 == ae.GetIndex())
            {
                ae.Stop();
                continue;
            }

            if (index == ae.GetIndex())
            {
                ae.Play();
            }
        }

        //General events
        foreach (GeneralEvent ge in unityEvents)
        {
            if (index == ge.GetIndex())
            {
                ge.Play();
            }
        }

        //Writing coroutine
        if (writing != null)
        {
            StopCoroutine(writing);
        }
        writing = StartCoroutine(WriteLine(index));

        //////////////// CLOSING //////////////// 
        Debug.Log("Closing step " + index);
        index++;

        //Activate new requirements
        foreach (RequirementEvent re in requirements)
        {
            if (index == re.GetIndex() && re.DoesActivateImmediately())
            {
                re.Activate();
            }
        }
    }

    IEnumerator WriteLine(int index)
    {
        if(index >= lines.Count)
        {
            yield break;
        }

        for(int i = 0; i <= lines[index].Length; i++)
        {
            subtitles.text = lines[index].Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }

        writing = null;
    }

    void SpeedUpWriting()
    {
        if (writing != null)
        {
            StopCoroutine(writing);
            writing = null;

            subtitles.text = lines[index - 1];
        }
    }

    public void FulfillRequirement(string identifier)
    {
        foreach(RequirementEvent re in requirements)
        {
            if (re.GetIdentifier().Equals(identifier))
            {
                re.FulfillCondition();
                break;
            }
        }
    }
}

[Serializable]
public class RequirementEvent
{
    [SerializeField]
    int index;

    [SerializeField]
    string identifier;

    [SerializeField]
    bool activateImmediately = true;

    [SerializeField]
    UnityEvent startEvent;

    [SerializeField]
    UnityEvent finishEvent;

    bool active = false;
    bool accomplished = false;


    public void Activate()
    {
        active = true;
        startEvent.Invoke();
    }

    public void FulfillCondition()
    {
        if (active)
        {
            accomplished = true;
            active = false;
            finishEvent.Invoke();
        }
    }

    public bool IsEventFulfilled()
    {
        return accomplished;
    }

    public bool DoesActivateImmediately()
    {
        return activateImmediately;
    }

    public int GetIndex()
    {
        return index;
    }

    public string GetIdentifier()
    {
        return identifier;
    }
}

[Serializable]
public class AudioEvent
{
    [SerializeField]
    AudioClip clip;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    int index;

    public void Play()
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public int GetIndex()
    {
        return index;
    }
}

[Serializable]
public class GeneralEvent
{
    [SerializeField]
    UnityEvent uEvent;

    [SerializeField]
    int index;

    public void Play()
    {
        uEvent.Invoke();
    }

    public int GetIndex()
    {
        return index;
    }
}
