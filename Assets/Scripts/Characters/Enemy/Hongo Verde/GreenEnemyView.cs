using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemyView : MonoBehaviour
{
    private GreenEnemy enemy;
    private Animator anim;

    public string[] triggersNames;
    public string[] boolsNames;

    void Awake()
    {
        enemy = GetComponent<GreenEnemy>();
        anim = GetComponent<Animator>();
    }

    public void ActivateTriggers(int index) => anim.SetTrigger(triggersNames[index]);

    public void ActivateBool(int index, bool value) => anim.SetBool(boolsNames[index], value);


}
