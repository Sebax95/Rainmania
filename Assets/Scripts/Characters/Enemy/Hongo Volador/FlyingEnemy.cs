using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float lifeTime;
    public bool contadorOrTime;
    public float minRandomAmplitud;
    public float maxRandomAmplitud;
    public float frecuency, amplitud;
    public int damage;
    
    private FSM<FlyingEnemy> _fsm;
    
    public GameObject deathParticle;

    protected override void Awake()
    {
        base.Awake();
        isDead = false;
        _fsm = new FSM<FlyingEnemy>(this);
        _fsm.AddState(StatesEnemies.Fly, new FlyState(this, _fsm));
        _fsm.SetState(StatesEnemies.Fly);
    }

    public override void Reset()
    {
        base.Reset();
        rb.useGravity = false;
        _fsm.SetState(StatesEnemies.Fly);
        StartCoroutine(DieTimer());  
    }

    protected override void Start()
    {
        base.Start();
        rb.useGravity = false;
        gameObject.SetActive(false);
    }
    
    public override void Move(Vector2 direction) => rb.velocity = direction;

    public void Move(Vector3 direction) => rb.velocity = direction;

    private void Update()
    {
        if (isDead) return;
        _fsm.Update();
    }

    private void FixedUpdate()
    {
        if(isDead) return;
        _fsm.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fly") || other.gameObject.layer == 0 || other.gameObject.layer == 9) return;
        var obj = other.gameObject.GetComponent<Character>();
        if (obj)
            obj.Damage(damage, this);
        StopCoroutine(DieTimer());
        Die(this);
        
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

        return result;
    }
    
    public override void Die(IDamager source)
    {
        isDead = true;
        viewEnem.PlaySound(EnemyView.AudioEnemys.Die);
        if(spawner)
            spawner.DestroyObject(this);
        else
            TurnOff(this);
        
        var part = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(part, 1);
    }
    
}