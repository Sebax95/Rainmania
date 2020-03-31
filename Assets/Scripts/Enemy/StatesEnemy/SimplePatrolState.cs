using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePatrolState : State<Enemy>
{
    public SimplePatrolState(Enemy owner, FSM<Enemy> fsm) : base(owner, fsm)
    {
    }

    private Vector3 startpos;
    private float distMax = 5f;
    public override void Enter()
    {
        startpos = _owner.transform.position;
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateState()
    {
        _owner.rb.MovePosition(_owner.transform.position + _owner.transform.right * _owner.speed * Time.deltaTime);
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(_owner.transform.position, startpos) >= distMax)
            _owner.transform.Rotate(Vector3.up, 180);
    }
}
