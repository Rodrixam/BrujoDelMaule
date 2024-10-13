using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrossPoint : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _visual;

    [SerializeField]
    Collider _collider;

    bool _completed = false;

    public void Activate()
    {
        _completed = false;

        _visual.enabled = true;
        _collider.enabled = true;
    }

    public void Deactivate()
    {
        _completed = false;

        _visual.enabled = false;
        _collider.enabled = false;
    }

    public SphereCollider GetCollider()
    {
        return _collider.GetComponent<SphereCollider>();
    }

    public bool IsCompleted()
    {
        return _completed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ola");
        if (other.tag.Equals("CrossCollider"))
        {
            _completed = true;
        }
    }
}
