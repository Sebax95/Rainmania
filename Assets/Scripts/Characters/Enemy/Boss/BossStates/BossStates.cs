using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BossMachine {

	public abstract class BossState<T> : FiniteState<T> {
		protected MushroomBoss.BossComponents components;
		protected List<BossRoot> _roots;

		public BossState(MushroomBoss.BossComponents bossComponents) : base(bossComponents.transform) {
			components = bossComponents;
		}

		public BossState(StateMachine<T> machine, MushroomBoss.BossComponents bossComponents) : base(machine) {
			components = bossComponents;
		}

	}

	public class BossInitialState<T> : BossState<T> {

		private float timeBeforeStateTransition;
		private float timeUntilTransition = 0;

		private T advanceFeed;

		public BossInitialState(MushroomBoss.BossComponents bossComponents) : base(bossComponents) { }

		public BossInitialState(StateMachine<T> machine, MushroomBoss.BossComponents bossComponents) : base(machine, bossComponents) { }

		public override void Enter() {
			base.Enter();
			timeUntilTransition = timeBeforeStateTransition;
		}

		public override void Update() {
			base.Update();
			timeUntilTransition -= Time.deltaTime;
			if(timeUntilTransition < 0)
				fsm.Feed(advanceFeed);
		}
	}

	public class BossIdleForRandomTimeState<T> : BossState<T> {

		private (float min, float max) timeRange;
		private float timeUntilSwitch;

		private T advanceFeed;

		public BossIdleForRandomTimeState(MushroomBoss.BossComponents bossComponents) : base(bossComponents) { }

		public BossIdleForRandomTimeState(StateMachine<T> machine, MushroomBoss.BossComponents bossComponents) :
			base(machine, bossComponents) { }

		public override void Enter() {
			base.Enter();
			timeUntilSwitch = Random.Range(timeRange.min, timeRange.max);
		}

		public override void Update() {
			base.Update();
			timeUntilSwitch -= Time.deltaTime;
			if(timeUntilSwitch < 0)
				fsm.Feed(advanceFeed);
		}

		#region Builder
		public BossIdleForRandomTimeState<T> SetTimeRange(float min, float max) {
			timeRange = (min, max);
			return this;
		}
		public BossIdleForRandomTimeState<T> SetAdvanceFeed(T feedItem) {
			advanceFeed = feedItem;
			return this;
		}
		#endregion
	}

	public class BossProxyToRandomState<T> : BossState<T> {

		public BossProxyToRandomState(MushroomBoss.BossComponents bossComponents) : base(bossComponents) {}

		public BossProxyToRandomState(StateMachine<T> machine, MushroomBoss.BossComponents bossComponents) :
			base(machine, bossComponents) {}

		public override void Enter() {
			base.Enter();
			int index = Random.Range(0, _transitions.Count);
			T keyToNext = _transitions.Keys.Skip(index).First();
			fsm.Feed(keyToNext);
		}
	}

	public class BossAttackWithRootState<T> : BossState<T> {

		private int rootAttackId = -1;

		public BossAttackWithRootState(MushroomBoss.BossComponents bossComponents) : base(bossComponents) {}
		public BossAttackWithRootState(StateMachine<T> machine, MushroomBoss.BossComponents bossComponents) : base(bossComponents) {}

		public BossAttackWithRootState<T> SetRootAttackId(int id) {
			rootAttackId = id;
			return this;
		}

		public override void Enter() {
			base.Enter();
		}
	}
}