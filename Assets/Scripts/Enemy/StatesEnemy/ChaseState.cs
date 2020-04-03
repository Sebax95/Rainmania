using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State<Enemy>
{
    public ChaseState(Enemy owner, FSM<Enemy> fsm) : base(owner, fsm)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateState()
    {
        _owner.rb.MovePosition(_owner.transform.position + _owner.transform.forward * _owner.speed * Time.deltaTime);
    }

    public override void UpdateState()
    {
        var dist = Vector3.Distance(_owner.transform.position, _owner.target.transform.position);
        if (dist < 4)
            Debug.Log("attackState");
        else
            _owner.transform.right = (_owner.target.transform.position - _owner.transform.position).normalized;
            //_owner.fsm.SetState()
    }
}
