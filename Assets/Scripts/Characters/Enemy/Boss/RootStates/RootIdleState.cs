using UnityEngine;
using BossMachine;

public class RootIdleState<T> : FiniteState<T> {
	public RootIdleState(Transform transform) : base(transform) {}
	public RootIdleState(StateMachine<T> owner) : base(owner) {}
}