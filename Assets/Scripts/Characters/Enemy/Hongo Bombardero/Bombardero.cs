using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bombardero : Enemy
{
    private bool _isDead;
    private bool _canShoot;
    public int damage;
    private FSM<Bombardero> _fsm;
    
    public GameObject bulletPref;

    public Vector2 leftPos;
    public Vector2 rightPos;

    public Vector3 offsetLeft;
    public Vector3 offsetRight;

    public GameObject deathParticle;

    public override void Reset()
    {    
        _isDead = false;
        rb.useGravity = false;
        offsetLeft = transform.position - (Vector3)leftPos;
        offsetRight = transform.position - (Vector3)rightPos;

        _fsm.SetState(StatesEnemies.Fly);
        StartCoroutine(ShootPlayer());
    }

    protected override void Awake()
    {
        base.Awake();
        _fsm = new FSM<Bombardero>(this);
        _fsm.AddState(StatesEnemies.Fly, new MovingState(this, _fsm));
        _isDead = false;
    }

    protected override void Start()
    {
        base.Start();
        rb.useGravity = false;
        offsetLeft = transform.position - (Vector3)leftPos;
        offsetRight = transform.position - (Vector3)rightPos;
        _fsm.SetState(StatesEnemies.Fly);
        StartCoroutine(ShootPlayer());
    }
    
    private void FixedUpdate()
    {
        if(_isDead) return;
        _fsm.FixedUpdate();
    }

    public override void Move(Vector2 direction) => rb.velocity = direction;

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

        var part = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(part, 1);
        
        if(spawner)
            spawner.DestroyObject(gameObject);
        else
            Destroy(gameObject);
    }

    IEnumerator ShootPlayer()
    {
        while (!_isDead)
        {
            viewEnem.ActivateTriggers(0);
            yield return new WaitForSeconds(cdTimer);
        }
    }

    public void Shoot()
    {
        var obj = Instantiate(bulletPref, output.transform.position, Quaternion.identity);
        obj.transform.forward = Vector3.down; 
        obj.GetComponent<PoisonBullet>().AssignTeam = GetTeam;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (!Application.isPlaying)
        {
            var offsetL = transform.position - (Vector3)leftPos;
            var offsetR = transform.position - (Vector3)rightPos;
            Gizmos.DrawSphere(offsetL, 0.2f);
            Gizmos.DrawSphere(offsetR, 0.2f);
        }
        else
        {
            Gizmos.DrawSphere(offsetLeft, 0.2f);
            Gizmos.DrawSphere(offsetRight, 0.2f);
        }
    }
}
