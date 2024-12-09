using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TioAnimationController : MonoBehaviour
{
    [SerializeField]
    Animator _tioAnimator;

    [SerializeField]
    Animator _crossAnimator;

    [SerializeField]
    CinematicController _cinematic;

    public void StartCross()
    {
        _tioAnimator.SetBool("cross", true);
        _crossAnimator.Play("CompletingCross");
    }

    public void FinishCross()
    {
        _tioAnimator.SetBool("cross", false);
        _cinematic.FulfillRequirement("Finish example");
    }

    public void StartSpeaking()
    {
        _tioAnimator.SetBool("speaking", true);
    }

    public void StopSpeaking()
    {
        _tioAnimator.SetBool("speaking", false);
    }
}