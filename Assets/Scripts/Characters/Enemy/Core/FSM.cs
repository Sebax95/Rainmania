﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    private T _owner;
    private Dictionary<StatesEnemies, State<T>> _states;
    private State<T> _currentState;

    public FSM(T owner)
    {
        _owner = owner;
        _states = new Dictionary<StatesEnemies, State<T>>();
    }

    public string ActualState => _currentState.ToString();

    public void AddState(StatesEnemies stateName, State<T> state)
    {
        _states.Add(stateName, state);
    }

    public void SetState(StatesEnemies stateName)
    {
        if (_currentState != null)
            _currentState.Exit();
        _currentState = _states[stateName];
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState.UpdateState();
    }

    public void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }
}
