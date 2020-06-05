using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateCaminante : State<HongoCaminante>
{
    public IdleStateCaminante(HongoCaminante owner, FSM<HongoCaminante> fsm) : base(owner, fsm)
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
        /*if (_owner.LineOfSight())
            _fsm.SetState(StatesEnemies.Walk);*/
    }
}
