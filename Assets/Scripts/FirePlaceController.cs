using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirePlaceController : MonoBehaviour
{
    [SerializeField]
    UnityEvent fireplaceEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Salt"))
        {
            fireplaceEvent.Invoke();
        }
    }
}
