using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;

public class PatrolState : State<GreenEnemy>
{
    public PatrolState(GreenEnemy owner, FSM<GreenEnemy> fsm) : base(owner, fsm)
    {
    }
    private bool isRight;
    private Vector3 startPos;

    public override void Enter()
    {
        startPos = _owner.transform.position;
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateState()
    {
        _owner.speed = (isRight) ? -5 : 5;
        _owner.Move(_owner.transform.position + Vector3.forward);
    }

    public override void UpdateState()
    {
         if(_owner.LineOfSight())
             _fsm.SetState(StatesGreenEnemy.Shoot);
         else
         {
            if (Vector3.Distance(_owner.transform.position, startPos) >= 5)
            {
                isRight = !isRight;
                _owner.transform.Rotate(new Vector3(_owner.transform.rotation.x, 180));
            }

         }

    }
}
