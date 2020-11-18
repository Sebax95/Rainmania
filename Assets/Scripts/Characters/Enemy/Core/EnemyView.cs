using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyView : TimedBehaviour
{

    private Animator anim;
    private AudioSource au;
    public Renderer mat;

    public string[] triggersNames;
    public string[] boolsNames;
    
    [Header("Audio")]
    [SerializeField]
    public List<AudioClip> audios = new List<AudioClip>();
    public List<AudioClip> stepRandom = new List<AudioClip>();
    public List<AudioClip> stepWaterRandom = new List<AudioClip>();

    public bool InWater { get; set;}
    private AudioClip RandomGroundStep => stepRandom[Random.Range(0, stepRandom.Count-1)];
    private AudioClip RandomWaterStep => stepRandom[Random.Range(0, stepWaterRandom.Count-1)];
    
    public enum AudioEnemys {
        Die, Attack, JumpingPad, Move
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
    /// Array Audios tiene que tener 4 si o si
    /// 0: Die
    /// 1: Attack
    /// 2: JumpingPad
    /// 3: Move
    /// </summary>
    /// <param name="sound"></param>
    public void PlaySound(AudioEnemys sound) {
        AudioClip selected = null;

        switch (sound)
        {
            case AudioEnemys.Die:
                selected = audios[0];
                break;
            case AudioEnemys.Attack:
                selected = audios[1];
                break;
            case AudioEnemys.JumpingPad:
                selected = audios[2];
                break;
           case AudioEnemys.Move:
                selected = InWater ? RandomWaterStep : RandomGroundStep;
                break;
        }

        if(selected)
            au?.PlayOneShot(selected);
    }

    public void Move() => PlaySound(AudioEnemys.Move);
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