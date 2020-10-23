﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{

    private Animator anim;
    private AudioSource au;
    public Renderer mat;

    public string[] triggersNames;
    public string[] boolsNames;
    
    [Header("Audio")]
    [SerializeField]
    public List<AudioClip> audios = new List<AudioClip>();

    public enum AudioEnemys {
        Hurt, Die, Shoot, JumpingPad
    }
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        mat = GetComponentInChildren<Renderer>();
    }

    public void ActivateTriggers(int index) => anim.SetTrigger(triggersNames[index]);

    public void ActivateBool(int index, bool value) => anim.SetBool(boolsNames[index], value);

    //public void SetAudioClip(AudioClip clip) => au.clip = clip;

    public AudioSource Au => au;

    public void DamageFeedback() => StartCoroutine(ChangeColor());
    
    
    /// <summary>
    /// 0: Hurt
    /// 1: Die
    /// 2: Shoot
    /// 3: JumpingPad
    /// </summary>
    /// <param name="sound"></param>
    public void PlaySound(AudioEnemys sound) {
        AudioClip selected = null;

        switch (sound)
        {
            case AudioEnemys.Hurt:
                selected = audios[0];
                break;
            case AudioEnemys.Die:
                selected = audios[1];
                break;
            case AudioEnemys.Shoot:
                selected = audios[2];
                break;
            case AudioEnemys.JumpingPad:
                selected = audios[3];
                break;
        }

        if(selected)
            au.PlayOneShot(selected);
    }

    IEnumerator ChangeColor()
    {
        var maxTime = 0.3f;
        var waitTime = 0f;
        var value = 0f;
        while (waitTime < maxTime)
        {
            value = Mathf.Lerp(0, 1, (waitTime /maxTime));
            waitTime += Time.deltaTime * 10;
            mat.material.SetFloat("_ColorLerp", value);

            yield return new WaitForSeconds(0.03f);
        }
        
        yield return new WaitForSeconds(0.5f);
        mat.material.SetFloat("_ColorLerp", 0);

    }
}