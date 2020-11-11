using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorZone : MonoBehaviour
{
    private EnemysRespawn parentObj;
    private BoxCollider col;
    void Start() => parentObj = GetComponentInParent<EnemysRespawn>();
    public BoxCollider SetCollider
    {
        get
        {
            if(!col)
                col = GetComponent<BoxCollider>();
            return col;
        }
        set
        {
            col.enabled = value;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var chara = other.GetComponent<Player>();
        if(!chara) return;
        parentObj.DetectPlayer(this);
    }
    
}
