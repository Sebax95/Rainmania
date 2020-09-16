using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{

    private Animator anim;
    private AudioSource au;
    public Renderer mat;

    public string[] triggersNames;
    public string[] boolsNames;

    void Awake()
    {
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        mat = GetComponentInChildren<Renderer>();
    }

    public void ActivateTriggers(int index) => anim.SetTrigger(triggersNames[index]);

    public void ActivateBool(int index, bool value) => anim.SetBool(boolsNames[index], value);

    public void SetAudioClip(AudioClip clip) => au.clip = clip;

    public AudioSource Au => au;

    public void DamageFeedback() => StartCoroutine(ChangeColor());

    IEnumerator ChangeColor()
    {

        var tempColor = mat.material.color;
        mat.material.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(.5f);
        mat.material.SetColor("_Color", tempColor);
    }
}