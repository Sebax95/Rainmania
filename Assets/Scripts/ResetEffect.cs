using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEffect : MonoBehaviour
{
    Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();   
    }

    private void OnEnable()
    {
        _anim.Play("Brillo");
    }
}
