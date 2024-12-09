using JetBrains.Annotations;
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
    bool startAutomatically = true;

    [SerializeField]
    float timeToStart = 0;

    [SerializeField]
    TextMeshProUGUI subtitles;

    [SerializeField]
    List<DialogueLine> lines = new List<DialogueLine>();

    [SerializeField]
    List<Animator> animators = new List<Animator>();

    [SerializeField]
    List<AudioEvent> audioEvents = new List<AudioEvent>();

    [SerializeField]
    List<RequirementEvent> requirements = new List<RequirementEvent>();

    [SerializeField]
    List<GeneralEvent> unityEvents = new List<GeneralEvent>();


    Coroutine writing = null;

    private void Awake()
    {
        inputTracker = FindAnyObjectByType<InputTracker>();
    }

    private void Start()
    {
        //Activate new requirements
        foreach (RequirementEvent re in requirements)
        {
            if (re.HasIndex(index) && re.DoesActivateImmediately())
            {
                re.Activate();
            }
        }

        if (startAutomatically)
        {
            Invoke("NextStep", timeToStart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputTracker.GetInputDown(ControllerButton.aButton))
        {
            if(writing == null)
            {
                NextStep();
            }
            FulfillRequirement("Show button");
        }
    }

    public void NextStep()
    {
        //////////////// OPENING //////////////// 
        Debug.Log("Opening step " + index);
        //Interrupt new step until requirements are fulfilled
        foreach (RequirementEvent re in requirements)
        {
            if (re.HasIndex(index) && !re.IsEventFulfilled())
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
            /*
            if (index == ae.GetIndex())
            {
                ae.Stop();
                continue;
            }//*/

            if (index == ae.GetIndex())
            {
                ae.Play();
            }
        }

        //General events
        foreach (GeneralEvent ge in unityEvents)
        {
            if (ge.HasIndex(index) && !ge.GetEndOfLine())
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
            if (re.HasIndex(index) && re.DoesActivateImmediately())
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

        for(int i = 0; i <= lines[index].GetLine().Length; i++)
        {
            subtitles.text = lines[index].GetLine().Substring(0, i);
            yield return new WaitForSeconds(lines[index].GetTime() / lines[index].GetLine().Length);
        }

        //General events
        foreach (GeneralEvent ge in unityEvents)
        {
            if (ge.HasIndex(index) && ge.GetEndOfLine())
            {
                ge.Play();
            }
        }

        writing = null;
    }

    /*
    void SpeedUpWriting()
    {
        if (writing != null)
        {
            StopCoroutine(writing);
            writing = null;

            subtitles.text = lines[index - 1].GetLine();

            //General events
            foreach (GeneralEvent ge in unityEvents)
            {
                if (ge.HasIndex(index - 1) && ge.GetEndOfLine())
                {
                    ge.Play();
                }
            }
        }
    }//*/

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
public class DialogueLine
{
    [SerializeField]
    string line;

    [SerializeField]
    float time;

    float startTime = -1;

    public void StartLine()
    {
        startTime = Time.time;
    }

    public string GetLine()
    {
        return line;
    }

    public float GetTime()
    {
        return time;
    }

    public bool IsCompleted()
    {
        if(startTime != -1 && (Time.time - startTime) >= time)
        {
            return true;
        }
        return false;
    }
}

[Serializable]
public class RequirementEvent
{
    [SerializeField]
    List<int> indexes;

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

    public bool HasIndex(int i)
    {
        return indexes.Contains(i);
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
    string description;

    [SerializeField]
    UnityEvent uEvent;

    [SerializeField]
    List<int> indexes;

    /// <summary>
    /// By default a GeneralEvent is played at the start of the line writing, check this to make it play when it finishes writing.
    /// </summary>
    [SerializeField]
    bool executeAtEndOfLine = false;

    public void Play()
    {
        uEvent.Invoke();
    }

    public bool HasIndex(int i)
    {
        return indexes.Contains(i);
    }

    public bool GetEndOfLine()
    {
        return executeAtEndOfLine;
    }
}
