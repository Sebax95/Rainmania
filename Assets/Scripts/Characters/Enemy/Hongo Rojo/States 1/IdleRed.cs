using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRed : State<RedEnemy>
{
    public IdleRed(RedEnemy owner, FSM<RedEnemy> fsm) : base(owner, fsm)
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

    }

    public override void UpdateState()
    {
        if (_owner.LineOfSight())
            _fsm.SetState(StatesEnemies.Shoot);

    }
}
