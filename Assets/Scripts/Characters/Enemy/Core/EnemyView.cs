using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{

    private Animator anim;

    public string[] triggersNames;
    public string[] boolsNames;

    void Awake() => anim = GetComponent<Animator>();    

    public void ActivateTriggers(int index) => anim.SetTrigger(triggersNames[index]);

    public void ActivateBool(int index, bool value) => anim.SetBool(boolsNames[index], value);


}
