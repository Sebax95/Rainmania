using UnityEngine;

namespace BossMachine {

	public abstract class BossState<T> : FiniteState<T> {
		protected Animator _animator;

		public BossState(Animator animator) : base() {
			_animator = animator;
		}

		public BossState(StateMachine<T> machine, Animator animator):base(machine) {
			_animator = animator;
		}

		public override void SetStateMachine(StateMachine<T> machine) => base.SetStateMachine(machine);
	}

	public class BossInitialState<T> : BossState<T> {

		public BossInitialState(Animator animator) : base(animator) {}

		public BossInitialState(StateMachine<T> machine, Animator animator) : base(machine, animator) {}

		public override void Enter() {
			base.Enter();
		}
	}
}