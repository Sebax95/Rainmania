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

		public void Feed(T feed, params object[] args) {
			FiniteState<T> next = currentState.GetTransition(feed);

			if(next != null)
			{
				currentState.Exit();
				currentState = next;
				currentState.Enter();
				currentState.PassArguments(args);
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

		public void FeedArguments(params object[] args) => currentState.PassArguments(args);
	}

	public abstract class FiniteState<T> {
		protected StateMachine<T> fsm;
		protected Transform transform;
		protected Dictionary<T, FiniteState<T>> _transitions = new Dictionary<T, FiniteState<T>>();

		public FiniteState(Transform transform) {
			this.transform = transform;
		}
		public FiniteState(StateMachine<T> owner) {
			fsm = owner;
			transform = owner.transform;
		}

		public virtual void Enter() { }
		public virtual void Update() { }
		public virtual void Exit() { }

		public FiniteState<T> GetTransition(T feed) {
			if(feed == null)
				return null;
			return _transitions.TryGetValue(feed, out var state) ? state : null;
		}

		public FiniteState<T> AddTransition(T feed, FiniteState<T> transitionTarget) {
			_transitions.Add(feed, transitionTarget);
			return this;
		}

		public virtual FiniteState<T> SetStateMachine(StateMachine<T> machine) {
			fsm = machine;
			transform = machine.transform;
			return this;
		}

		public virtual void PassArguments(params object[] args) { }
	}
}