using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SaltBagController : MonoBehaviour
{
    [SerializeField]
    Transform _anchor;

    [SerializeField]
    Transform _leftController, _rightController;

    [SerializeField]
    InputAction _leftAction, _rightAction;

    [SerializeField]
    GameObject _saltHandful;

    CONTROLLER grabState = CONTROLLER.none;
    public bool canPullSalt = false;

    GameObject sAux = null;

    // Update is called once per frame
    void Update()
    {
        if(grabState != CONTROLLER.none && canPullSalt && sAux == null && _leftAction.ReadValue<float>() > 0)
        {
            if(grabState == CONTROLLER.left)
            {
                sAux = Instantiate(_saltHandful, _leftController);
            }
            else if(grabState == CONTROLLER.right)
            {
                sAux = Instantiate(_saltHandful, _leftController);
            }
        }

        if(sAux != null && _leftAction.ReadValue<float>() == 0)
        {
            sAux.transform.parent = null;
        }
    }

    public void StartGrab()
    {
        if (_anchor.position == _leftController.position)
        {
            grabState = CONTROLLER.left;
        }
        else if(_anchor.position == _rightController.position)
        {
            grabState = CONTROLLER.right;
        }
    }

    public void EndGrab()
    {
        grabState = CONTROLLER.none;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("CrossCollider"))
        {
            canPullSalt = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("CrossCollider"))
        {
            canPullSalt = false;
        }
    }
}