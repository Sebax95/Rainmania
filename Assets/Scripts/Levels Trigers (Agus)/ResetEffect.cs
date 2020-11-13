using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEffect : TimedBehaviour
{
    Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();   
    }

    private void OnEnable()
    {
        if(_anim != null)
        _anim.Play("Brillo");
    }
}
