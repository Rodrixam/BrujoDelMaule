using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;

public class CrossController : MonoBehaviour
{

    [SerializeField]
    CrossPoint up, down, left, right;
    [SerializeField]
    Transform _rightHandTransform;
    CrossPoint[] _crossPoints;

    [Header("Input")]
    [SerializeField]
    InputAction _action;
    InputTracker inputTracker;

    [Header("Output")]
    [SerializeField]
    UnityEvent CrossMimic = new UnityEvent();

    [Header("Animation")]
    [SerializeField]
    Animator _handAnimator;
    LineRenderer _lineRenderer;
    Vector3[] _linePositions = new Vector3[4];

    [Header("Debug")]
    int pointIndex = 0;
    bool buttonPressed = false;
    public bool canMimic = true;


    private void OnEnable()
    {
        _action.Enable();
    }
    private void OnDisable()
    {
        _action.Disable();
    }

    private void Awake()
    {
        inputTracker = FindAnyObjectByType<InputTracker>();
    }

    void Start()
    {
        //Get components
        _lineRenderer = GetComponent<LineRenderer>();
        _linePositions = GetCrossPointArray();

        //Deactivates all points
        _crossPoints = new CrossPoint[4] { up, down, left, right };
        foreach(CrossPoint cs in _crossPoints)
        {
            cs.Deactivate();
        }
    }

    void Update()
    {
        //Get input value
        if (canMimic && !buttonPressed && _action.ReadValue<float>() == 1)
        {
            PressButton();
        }
        else if(!canMimic || (buttonPressed && _action.ReadValue<float>() == 0))
        {
            ReleaseButton();
        }

        //Go to next step in mimic
        if (canMimic && buttonPressed)
        {
            _lineRenderer.SetPosition(pointIndex + 1, _rightHandTransform.position);

            _linePositions = GetCrossPointArray();
            for(int i = pointIndex; i >= 0; i--)
            {
                _lineRenderer.SetPosition(i, _linePositions[i]);
            }

            if (_crossPoints[pointIndex].IsCompleted())
            {
                _crossPoints[pointIndex].Deactivate();
                pointIndex++;
                //_lineRenderer.positionCount = pointIndex + 2;

                if (pointIndex >= 4)
                {
                    CrossMimic.Invoke();
                    ReleaseButton();
                }
                else
                {
                    _crossPoints[pointIndex].Activate();

                    _lineRenderer.SetPosition(pointIndex, _linePositions[pointIndex]);
                }
            }
        }

        //Debug
        if (inputTracker.GetInput(ControllerButton.leftTrigger) && inputTracker.GetInputDown(ControllerButton.rightTrigger))
        {
            CrossMimic.Invoke();
        }

        if (inputTracker != null && inputTracker.GetInputDown(ControllerButton.rightTrigger))
        {
            SendRaycast();
        }
    }

    Vector3[] GetCrossPointArray()
    {
        Vector3[] result = new Vector3[]
        {
            up.transform.position,
            down.transform.position,
            left.transform.position,
            right.transform.position,
        };

        return result;
    }

    public Vector3[] GetLineRendererPoints()
    {
        Vector3[] result = new Vector3[4];
        _lineRenderer.GetPositions(result);
        return result;
    }

    public void PressButton()
    {
        buttonPressed = true;

        pointIndex = 0;
        _crossPoints[pointIndex].Activate();

        if (_handAnimator != null)
        {
            _handAnimator.SetBool("DoingCross", true);
        }
        /*
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(pointIndex, _linePositions[pointIndex]);
        _lineRenderer.SetPosition(pointIndex + 1, _linePositions[pointIndex]);//*/
    }

    public void ReleaseButton() 
    {
        buttonPressed = false;

        foreach (CrossPoint cs in _crossPoints)
        {
            cs.Deactivate();
        }

        if(_handAnimator != null)
        {
            _handAnimator.SetBool("DoingCross", false);
        }

        _lineRenderer.positionCount = 0;
    }

    public float ActionValue()
    {
        return _action.ReadValue<float>();
    }

    public void SendRaycast()
    {
        RaycastHit output;
        Physics.Raycast(transform.position, transform.forward, out output, 100);

        if (output.collider != null &&  output.collider.CompareTag("Brujo"))
        {
            output.collider.gameObject.GetComponent<OutsideBrujoController>().TryCross();
        }

        Debug.DrawRay(transform.position, transform.forward * output.distance, Color.yellow);
    }

    /*
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Gizmos.DrawLine(origin, origin + transform.forward);
    }//*/
}
