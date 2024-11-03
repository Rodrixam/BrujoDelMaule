using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedDoorController : MonoBehaviour
{
    [SerializeField]
    Transform doorGlobal, doorBody;

    [SerializeField]
    MeshRenderer doorMaterial;

    [SerializeField]
    Material _defaultMat, _grabMat, _movingMat;

    InputTracker inputTracker;

    CONTROLLER handRef = CONTROLLER.none;
    [SerializeField]
    Transform handTransform = null;

    [SerializeField]
    bool grabbable = false;
    bool stillGrabbed = false;

    [SerializeField]
    bool moving = false;

    private void Awake()
    {
        inputTracker = GameObject.Find("DebugController").GetComponent<InputTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable || stillGrabbed)
        {
            if((handRef == CONTROLLER.left && inputTracker.GetInput(ControllerButton.leftGrip)) || (handRef == CONTROLLER.right && inputTracker.GetInput(ControllerButton.rightGrip)) || moving)
            {
                stillGrabbed = true;

                Vector3 dir = handTransform.position - doorBody.position;
                float angle = Vector3.SignedAngle(Quaternion.AngleAxis(doorGlobal.rotation.eulerAngles.y, Vector3.up) * Vector3.forward, dir, Vector3.up);

                Debug.Log(dir + " " + Quaternion.AngleAxis(doorGlobal.rotation.eulerAngles.y, Vector3.up) * Vector3.forward + " " + angle);

                if(angle > 0)
                {
                    doorBody.localRotation = Quaternion.Euler(doorBody.eulerAngles.x, angle, doorBody.eulerAngles.z);
                }
                else
                {
                    doorBody.localRotation = Quaternion.Euler(doorBody.eulerAngles.x, 0, doorBody.eulerAngles.z);
                }

                doorMaterial.material = _movingMat;
            }
            else
            {
                stillGrabbed = false;
                doorMaterial.material = _grabMat;

                if (!grabbable)
                {
                    handRef = CONTROLLER.none;
                    handTransform = null;

                    doorMaterial.material = _defaultMat;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrossCollider"))
        {
            grabbable = true;
            handRef = other.GetComponent<PositionSphereController>()._ref;
            handTransform = other.transform;


            doorMaterial.material = _grabMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CrossCollider"))
        {
            grabbable = false;

            if (!stillGrabbed)
            {
                handRef = CONTROLLER.none;
                handTransform = null;

                doorMaterial.material = _defaultMat;
            }
        }
    }
}