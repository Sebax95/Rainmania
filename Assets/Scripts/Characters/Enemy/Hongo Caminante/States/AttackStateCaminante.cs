using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateCaminante : State<HongoCaminante>
{
    public AttackStateCaminante(HongoCaminante owner, FSM<HongoCaminante> fsm) : base(owner, fsm)
    {
    }

    public override void Enter()
    {
        _owner.viewEnem.ActivateBool(0, false);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        if (_owner.LineOfSight())
        {
            if (_owner.canJump)
            {
                _owner.canJump = false;
                _owner.StartCoroutine(_owner.Jump());
            }
        }
        else if (!_owner.LineOfSight() && _owner.canJump)
            _fsm.SetState(StatesEnemies.Walk);
    }
}
