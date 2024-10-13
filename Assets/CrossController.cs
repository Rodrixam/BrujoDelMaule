using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;

public class CrossController : MonoBehaviour
{
    [SerializeField]
    CrossPoint up, down, left, right;
    CrossPoint[] _crossPoints;

    int pointIndex = 0;
    bool buttonPressed = false;

    [SerializeField]
    UnityEvent CrossMimic = new UnityEvent();

    [SerializeField]
    InputAction _action;

    // Start is called before the first frame update
    void Start()
    {
        _crossPoints = new CrossPoint[4] { up, down, left, right };
        foreach(CrossPoint cs in _crossPoints)
        {
            cs.Deactivate();
        }
    }

    private void OnEnable()
    {
        _action.Enable();
    }

    private void OnDisable()
    {
        _action.Disable();
    }


    void Update()
    {
        if (_action.ReadValue<float>() == 1)
        {
            PressButton();
        }
        else if(_action.ReadValue<float>() == 0)
        {
            ReleaseButton();
        }

        if (buttonPressed && _crossPoints[pointIndex].IsCompleted())
        {
            _crossPoints[pointIndex].Deactivate();
            pointIndex++;
            if(pointIndex >= 4)
            {
                CrossMimic.Invoke();
                ReleaseButton();
            }
            else
            {
                _crossPoints[pointIndex].Activate();
            }
        }
        
    }

    public void PressButton()
    {
        buttonPressed = true;

        pointIndex = 0;
        _crossPoints[pointIndex].Activate();
    }

    public void ReleaseButton() 
    {
        buttonPressed = false;

        foreach (CrossPoint cs in _crossPoints)
        {
            cs.Deactivate();
        }
    }
}
