using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptedDoorAnimation : MonoBehaviour
{
    InputTracker inputTracker;

    CONTROLLER handRef = CONTROLLER.none;

    [SerializeField]
    bool grabbable = false;

    [SerializeField]
    UnityEvent _output;

    private void Awake()
    {
        inputTracker = FindAnyObjectByType<InputTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable && ((handRef == CONTROLLER.left && inputTracker.GetInput(ControllerButton.leftGrip)) || (handRef == CONTROLLER.right && inputTracker.GetInput(ControllerButton.rightGrip))))
        {
            _output.Invoke();
        }

        if (inputTracker.GetInput(ControllerButton.leftTrigger) && inputTracker.GetInputDown(ControllerButton.rightTrigger))
        {
            _output.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrossCollider"))
        {
            grabbable = true;
            handRef = other.GetComponent<PositionSphereController>()._ref;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CrossCollider"))
        {
            grabbable = false;
            handRef = CONTROLLER.none;
        }
    }
}
