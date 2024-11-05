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

            if (_crossPoints[pointIndex].IsCompleted())
            {
                _crossPoints[pointIndex].Deactivate();
                pointIndex++;

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

        if (FindAnyObjectByType<InputTracker>().GetInputDown(ControllerButton.rightTrigger))
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

        _lineRenderer.SetPosition(pointIndex, _linePositions[pointIndex]);
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
