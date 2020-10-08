using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State<Bombardero>
{
    public MovingState(Bombardero owner, FSM<Bombardero> fsm) : base(owner, fsm) { }
    
    private float _distL;
    private float _distR;
    private bool _canFlip;
    private Vector3 target;

    public override void Enter()
    {
        target = (_owner.offsetLeft - _owner.transform.position).normalized;
    }

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        Moving();
    }

    private void Moving()
    {
        _distL = (_owner.offsetLeft - _owner.transform.position).sqrMagnitude;
       _distR = (_owner.offsetRight - _owner.transform.position).sqrMagnitude;

       if (_distR < 0.2f)
       {
           _canFlip = true;
           target = (_owner.offsetLeft - _owner.transform.position).normalized;
       }
       if (_distL < 0.2f)
       {
           _canFlip = false;
           target = (_owner.offsetRight - _owner.transform.position).normalized;
       }

       _owner.transform.forward = target;
       _owner.Move(_owner.transform.forward + target * _owner.speed);
       
    }

    public override void Exit()
    {
        
    }
}
