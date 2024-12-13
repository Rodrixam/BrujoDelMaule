using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFollower : MonoBehaviour
{
    [SerializeField]
    Transform _camera;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, _camera.rotation.eulerAngles.y, 0);
        transform.position = new Vector3(_camera.position.x, _camera.position.y, _camera.position.z);
    }
}
