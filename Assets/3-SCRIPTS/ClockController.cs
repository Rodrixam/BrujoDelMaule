using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField]
    Transform hour, minute;

    [SerializeField]
    bool automatic = false;

    float maxTime;
    float currentTime = 0;

    private void Update()
    {
        if (automatic)
        {
            minute.Rotate(0, 1f  * Time.deltaTime, 0, Space.Self);
            hour.Rotate(0, 1f/60f * Time.deltaTime, 0, Space.Self);
        }
        else
        {
            currentTime += Time.deltaTime;
            SetTime(currentTime);
        }
    }

    public void SetMaxTime(float maxTime)
    {
        this.maxTime = maxTime;
    }

    public void SetTime(float currentTime)
    {
        float normTime = maxTime / currentTime;

        minute.localRotation = Quaternion.Euler(minute.localRotation.eulerAngles.x, 90 + 2160 * normTime, minute.localRotation.eulerAngles.z);
        hour.localRotation = Quaternion.Euler(minute.localRotation.eulerAngles.x, 90 + 180 * normTime, hour.localRotation.eulerAngles.z);
    }
}
