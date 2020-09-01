using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombardero : Enemy
{
    private bool _isDead;
    public int damage;
    private FSM<Bombardero> _fsm;

    public float maxDistance;
    public Vector3 startPos;

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
        startPos = transform.position;
        _fsm.SetState(StatesEnemies.Fly);
    }
    
    private void FixedUpdate()
    {
        if(_isDead) return;
        _fsm.FixedUpdate();
    }
    
    public override void Move(Vector2 direction) => rb.velocity = direction;

    public override void Die(IDamager source)
    {
        
    }
    
}
