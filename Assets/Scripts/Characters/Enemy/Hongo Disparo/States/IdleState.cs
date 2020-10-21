using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<MushroomEnemy>
{
    public IdleState(MushroomEnemy owner, FSM<MushroomEnemy> fsm) : base(owner, fsm) { }
    public override void Enter() { }
    public override void Exit() { }
    public override void FixedUpdateState() { }
    public override void UpdateState()
    {
        if (_owner.LineOfSight())
            _fsm.SetState(StatesEnemies.Shoot);
    }
}
