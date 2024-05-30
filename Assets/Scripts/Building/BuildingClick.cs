using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClick : MonoBehaviour
{
    private Animator _animator;
   [SerializeField] private Animator[] extraAnimators;
    private bool _canClick = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool Click()
    {
        if (!_canClick) return false;
        _canClick = false;
        _animator.Play("Click");
        foreach (Animator anim in extraAnimators)
        {
            anim.Play("Click");
        }
        return true;
    }

    public void OnAnimationEnd() => _canClick = true; //llamado por un evento de animacion
}
