using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBoolSetter : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetBoolTrue(string boolName)
    {
        _animator.SetBool(boolName, true);
    }
}
