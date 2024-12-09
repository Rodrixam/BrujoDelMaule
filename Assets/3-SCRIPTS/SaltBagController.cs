using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SaltBagController : MonoBehaviour
{
    [SerializeField]
    Transform _anchor;

    [SerializeField]
    Transform _leftController, _rightController;

    [SerializeField]
    InputAction _leftAction, _rightAction;

    [SerializeField]
    NearFarInteractor _leftInteractor, _rightInteractor;

    [SerializeField]
    GameObject _saltHandful;

    CONTROLLER grabState = CONTROLLER.none;
    public bool canPullSalt = false;

    GameObject saltRef = null;

    [SerializeField]
    Material _defaultMat, _leftMat, _rightMat, _pullMat;

    [SerializeField]
    MeshRenderer _mesh;


    private void OnEnable()
    {
        _leftAction.Enable();
        _rightAction.Enable();
    }
    private void OnDisable()
    {
        _leftAction.Disable();
        _rightAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(grabState != CONTROLLER.none && canPullSalt && saltRef == null)
        {
            if(grabState == CONTROLLER.left && _rightAction.ReadValue<float>() > 0)
            {
                saltRef = Instantiate(_saltHandful, _rightController);
            }
            else if(grabState == CONTROLLER.right && _leftAction.ReadValue<float>() > 0)
            {
                saltRef = Instantiate(_saltHandful, _leftController);
            }
        }

        if(saltRef != null)
        {
            if ((grabState == CONTROLLER.left && _rightAction.ReadValue<float>() == 0) || (grabState == CONTROLLER.right && _leftAction.ReadValue<float>() == 0))
            {
                saltRef.transform.parent = null;
            }

            if (!saltRef.activeSelf)
            {
                saltRef = null;
            }
        }
    }

    public void CallGrab()
    {
        Invoke("StartGrab", 0.1f);
    }

    void StartGrab()
    {
        if (_anchor.position == _leftController.position)
        {
            grabState = CONTROLLER.left;
            _rightInteractor.interactionLayers = InteractionLayerMask.NameToLayer("Default");
            GetComponent<MeshRenderer>().material = _leftMat;
        }
        else if(_anchor.position == _rightController.position) 
        {
            grabState = CONTROLLER.right;
            _leftInteractor.interactionLayers = InteractionLayerMask.NameToLayer("Default");
            GetComponent<MeshRenderer>().material = _rightMat;
        }
    }

    public void EndGrab()
    {
        grabState = CONTROLLER.none;
        _leftInteractor.interactionLayers = InteractionLayerMask.GetMask("Default", "Salt");
        _rightInteractor.interactionLayers = InteractionLayerMask.GetMask("Default", "Salt");
        GetComponent<MeshRenderer>().material = _defaultMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (grabState != CONTROLLER.none && other.tag.Equals("CrossCollider"))
        {
            canPullSalt = true;
            GetComponent<MeshRenderer>().material = _pullMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("CrossCollider"))
        {
            canPullSalt = false;

            if (grabState == CONTROLLER.left)
            {
                GetComponent<MeshRenderer>().material = _leftMat;
            }
            else if (grabState == CONTROLLER.right)
            {
                GetComponent<MeshRenderer>().material = _rightMat;
            }
            else
            {
                GetComponent<MeshRenderer>().material = _defaultMat;
            }
        }
    }

    public List<bool> CanSpawnSalt()
    {
        List<bool> answers = new List<bool>();

        answers.Add(grabState != CONTROLLER.none);
        answers.Add(canPullSalt);
        answers.Add(saltRef == null);

        if (grabState == CONTROLLER.left)
        {
            answers.Add(_rightAction.ReadValue<float>() > 0);
        }
        else if (grabState == CONTROLLER.right)
        {
            answers.Add(_leftAction.ReadValue<float>() > 0);
        }

        return answers;
    }

    public float[] ActionValues()
    {
        return new float[2] { _leftAction.ReadValue<float>(), _rightAction.ReadValue<float>() };
    }

    public bool HasSaltSpawned()
    {
        if(saltRef == null)
        {
            return false;
        }
        return true;
    }

    public Vector3? SaltPosition()
    {
        if(saltRef == null)
        {
            return null;
        }
        return saltRef.transform.position;
    }
}