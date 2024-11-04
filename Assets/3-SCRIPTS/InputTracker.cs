using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTracker : MonoBehaviour
{
    [SerializeField]
    InputAction rightTrigger, leftTrigger, rightGrip, leftGrip, aButton, bButton, xButton, yButton;

    private List<InputAction> inputs = new List<InputAction>();
    private List<bool> previousStates = new List<bool>();
    private List<bool> currentStates = new List<bool>();

    private void OnEnable()
    {
        foreach(InputAction action in inputs)
        {
            action.Enable();
        }
    }
    private void OnDisable()
    {
        foreach (InputAction action in inputs)
        {
            action.Disable();
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        rightTrigger.Rename("rightTrigger");
        inputs.Add(rightTrigger);
        previousStates.Add(false);
        currentStates.Add(false);

        leftTrigger.Rename("leftTrigger");
        inputs.Add(leftTrigger);
        previousStates.Add(false);
        currentStates.Add(false);

        rightGrip.Rename("rightGrip");
        inputs.Add(rightGrip);
        previousStates.Add(false);
        currentStates.Add(false);

        leftGrip.Rename("leftGrip");
        inputs.Add(leftGrip);
        previousStates.Add(false);
        currentStates.Add(false);

        aButton.Rename("aButton");
        inputs.Add(aButton);
        previousStates.Add(false);
        currentStates.Add(false);

        bButton.Rename("bButton");
        inputs.Add(bButton);
        previousStates.Add(false);
        currentStates.Add(false);

        xButton.Rename("xButton");
        inputs.Add(xButton);
        previousStates.Add(false);
        currentStates.Add(false);

        yButton.Rename("yButton");
        inputs.Add(yButton);
        previousStates.Add(false);
        currentStates.Add(false);
    }

    void SetInputState()
    {
        for(int i = 0; i < inputs.Count; i++)
        {
            previousStates[i] = currentStates[i];

            if (inputs[i].ReadValue<float>() > 0)
            {
                currentStates[i] = true;
            }
            else
            {
                currentStates[i] = false;
            }
        }
    }

    public bool GetInputDown(ControllerButton input)
    {
        if (!previousStates[(int)input] && currentStates[(int)input])
        {
            return true;
        }
        return false;
    }

    public bool GetInput(ControllerButton input)
    {
        if (currentStates[(int)input])
        {
            return true;
        }
        return false;
    }

    public bool GetInputUp(ControllerButton input)
    {
        if (previousStates[(int)input] && !currentStates[(int)input])
        {
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        SetInputState();
    }

    private void Update()
    {
        SetInputState();
    }
}

public enum ControllerButton
{
    rightTrigger = 0, leftTrigger = 1, rightGrip = 2, leftGrip = 3, aButton = 4, bButton = 5, xButton = 6, yButton = 7
}
