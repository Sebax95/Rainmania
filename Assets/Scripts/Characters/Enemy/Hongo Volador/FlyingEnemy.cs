using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy, IDamager
{
    public GameObject SourceObject => gameObject;
    public float lifeTime;
    
    protected override void Awake()
    {
        base.Awake();
        rb.useGravity = false;
    }
    
    
    
    public override void Die(IDamager source)
    {
        spawner.DestroyObject(this.gameObject);
    }
    
}
