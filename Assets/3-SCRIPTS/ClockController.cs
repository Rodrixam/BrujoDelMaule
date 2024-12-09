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

    [SerializeField]
    float maxTime;
    [SerializeField] 
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

    //Called from GameController
    public void SetMaxTime(float maxTime)
    {
        this.maxTime = maxTime;
    }

    public void SetTime(float currentTime)
    {
        float normTime = currentTime / maxTime;

        minute.localRotation = Quaternion.Euler(90, 0, -90);
        hour.localRotation = Quaternion.Euler(90, 0, -90);

        minute.Rotate(0, normTime * 2160, 0, Space.Self);
        hour.Rotate(0, normTime * 180, 0, Space.Self);//*/
    }
}
