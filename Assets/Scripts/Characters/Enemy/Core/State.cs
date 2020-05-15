using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void Exit();

    protected T _owner;
    protected FSM<T> _fsm;

    protected State(T owner, FSM<T> fsm)
    {
        _owner = owner;
        _fsm = fsm;
    }
}
