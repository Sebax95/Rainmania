using System.Collections.Generic;
using UnityEngine;

namespace BossMachine {

	public class StateMachine<T> {

		public Transform transform { get; protected set; }
		protected FiniteState<T> currentState;
		public FiniteState<T> CurrentState { get { return currentState; } }

		public StateMachine() { }
		public StateMachine(Transform transform) {
			this.transform = transform;
		}

		public StateMachine(FiniteState<T> initialState, Transform transform = null) {
			this.transform = transform;
			this.currentState = initialState;
			this.currentState.Enter();
		}


		public void Feed(T feed) {
			FiniteState<T> next = currentState.GetTransition(feed);

			if(next != null)
			{
				currentState.Exit();
				currentState = next;
				currentState.Enter();
			}
		}

		public void Update() {
			currentState?.Update();
		}

		public void Initialize(FiniteState<T> initialState) {
			if(currentState != null)
				throw new System.InvalidOperationException("This state machine is already initialized");
			currentState = initialState;
			currentState.Enter();
		}
	}

	public abstract class FiniteState<T> {
		protected StateMachine<T> fsm;
		protected Transform transform;
		protected Dictionary<T, FiniteState<T>> _transitions = new Dictionary<T, FiniteState<T>>();

		public FiniteState(){}
		public FiniteState(StateMachine<T> owner) {
			fsm = owner;
			transform = owner.transform;
		}

		public virtual void Enter() {}
		public virtual void Update() {}
		public virtual void Exit() {}

		public FiniteState<T> GetTransition(T feed) =>
			_transitions.TryGetValue(feed, out var state) ? state : null;

		public void AddTransition(T feed, FiniteState<T> transitionTarget) => _transitions.Add(feed, transitionTarget);
		public virtual void SetStateMachine(StateMachine<T> machine) {
			fsm = machine;
			transform = machine.transform;
		}
	}
}