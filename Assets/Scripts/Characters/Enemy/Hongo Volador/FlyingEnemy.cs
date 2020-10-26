﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float lifeTime;
    public float minRandomAmplitud;
    public float maxRandomAmplitud;
    public float frecuency, amplitud;
    public int damage;

    private bool _isDead;
    private FSM<FlyingEnemy> _fsm;
    
    public GameObject deathParticle;

    protected override void Awake()
    {
        base.Awake();
        _isDead = false;
        _fsm = new FSM<FlyingEnemy>(this);
        _fsm.AddState(StatesEnemies.Fly, new FlyState(this, _fsm));
        _fsm.SetState(StatesEnemies.Fly);
    }

    public override void Reset()
    {
        isDead = false;
        rb.useGravity = false;
        _fsm.SetState(StatesEnemies.Fly);
    }

    protected override void Start()
    {
        base.Start();
        rb.useGravity = false;
        StartCoroutine(DieTimer());
    }
    
    public override void Move(Vector2 direction) => rb.velocity = direction;

    public void Move(Vector3 direction) => rb.velocity = direction;

    private void FixedUpdate()
    {
        if(_isDead) return;
        _fsm.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<Character>();
        if (obj)
        {
            obj.Damage(damage, this);
            Die(this);
        }
    }

    IEnumerator DieTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Die(this);
    }
    
    public override bool Damage(int amount, IDamager source)
    {
        var result = base.Damage(amount, source);
        if (!result) return result;
        viewEnem.DamageFeedback();

        return result;
    }
    
    public override void Die(IDamager source)
    {
        _isDead = true;

        if(spawner)
            spawner.DestroyObject(gameObject);
        else
            Destroy(gameObject);
        
        var part = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(part, 1);
    }
    
}