using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;

public class ShootState : State<GreenEnemy>
{
    public ShootState(GreenEnemy owner, FSM<GreenEnemy> fsm) : base(owner, fsm)
    {
    }


    public override void Enter()
    {
        _owner.canShoot = true;
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
           _owner.Shoot();
        else
            _fsm.SetState(StatesGreenEnemy.Idle);
    }



}
