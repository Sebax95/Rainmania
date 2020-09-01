using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State<Bombardero>
{
    public MovingState(Bombardero owner, FSM<Bombardero> fsm) : base(owner, fsm) { }

    private bool _isRight;
    private Vector3 _move;
    private float _dist;
    
    public override void Enter() { }

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        Moving();
    }

    void Moving()
    {
        _owner.Move(_owner.transform.forward);

       /* _dist = (_owner.startPos - _owner.transform.position).sqrMagnitude;
        Debug.Log(_dist);
        /*if (_dist > _owner.maxDistance) 
        {
            //_owner.transform.rotation = Quaternion.Euler(0, -90, 0);
            _owner.speed *= -1;
        }
        /*else
        {
            _owner.transform.rotation = Quaternion.Euler(0, 90, 0);
            _owner.speed *= -1;
        }*/
    }

    public override void Exit()
    {
        
    }
}
