using System;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;
using BossMachine;

public class BossRoot : MonoBehaviour, IAnimationEndActivable, IDamager, IDamageable {


	public const string STATE_DIE = "StateDie";
	public const string STATE_ADVANCE = "AdvanceState";
	public const string STATE_ATTACK_PREFIX = "Attack";
	public const string STATE_RISE = "StateRise";
	public const string STATE_SINK = "StateSink";


	[SerializeField]
	[Tooltip("0: Front. 1: Side")]
	[Range(0, 1)]
	private int rootType;
	[SerializeField]
	private float sinkRiseSpeed;
	[SerializeField]
	private float riseHeight;

	private Animator _animator;
	private Renderer _renderer;
	private StateMachine<string> _fsm;
	private Team team;
	[SerializeField] private MushroomBoss boss;
	public MushroomBoss.BossComponents components;

	public List<BoxCastParams> hitboxes;
	public List<float> attackDelays;

	public static string GetAttackFeed(int id) => STATE_ATTACK_PREFIX + id.ToString();

	private void Awake() {
		var child = transform.GetChild(0);
		_animator = child.GetComponent<Animator>();
		_renderer = child.GetComponent<Renderer>();
		InitiatializeStateMachine(rootType);
	}

	// Start is called before the first frame update
	void Start() {
		//_renderer.enabled = false;
		InitiatializeStateMachine(rootType);
	}

	// Update is called once per frame
	void Update() {
		_fsm.Update();
	}



	/// <param name="type">0: front. 1: side</param>
	private void InitiatializeStateMachine(int type) {
		_fsm = new StateMachine<string>(transform);

		if(type == 0)
			InitFront();
		else
			InitSide();

		void InitFront() {
			var rise = CreateRiseSinkState(true);
			var idle = new RootIdleState<string>(transform);
			var stomp = CreateAttackState(0);
			var swipe = CreateAttackState(3);
			var sink = CreateRiseSinkState(false);

			rise.AddTransition(STATE_ADVANCE, idle);
			sink.AddTransition(STATE_ADVANCE, idle);

			idle.AddTransition(STATE_RISE, rise);
			idle.AddTransition(STATE_SINK, sink);
			idle.AddTransition(GetAttackFeed(0), stomp);
			idle.AddTransition(GetAttackFeed(3), swipe);

			stomp.AddTransition(STATE_ADVANCE, idle);
			swipe.AddTransition(STATE_ADVANCE, idle);

			_fsm.Initialize(idle);
		}

		void InitSide() {
			var rise = CreateRiseSinkState(true);
			var idle = new RootIdleState<string>(transform);
			var slam = CreateAttackState(1);
			var stab = CreateAttackState(2);
			var sink = CreateRiseSinkState(false);

			rise.AddTransition(STATE_ADVANCE, idle);
			sink.AddTransition(STATE_ADVANCE, idle);

			idle.AddTransition(STATE_RISE, rise);
			idle.AddTransition(STATE_SINK, sink);
			idle.AddTransition(GetAttackFeed(1), slam);
			idle.AddTransition(GetAttackFeed(2), stab);

			slam.AddTransition(STATE_ADVANCE, idle);
			stab.AddTransition(STATE_ADVANCE, idle);

			_fsm.Initialize(idle);
		}
	}

	private FiniteState<string> CreateRiseSinkState(bool isRiseNotSink) {
		FiniteState<string> state;
		if(isRiseNotSink)
		{
			state = new RootRiseState<string>(transform)
				.SetAnimator(_animator)
				.SetRisePos(transform.position.ZeroY() + Vector3.up * riseHeight)
				.SetRiseDuration(sinkRiseSpeed)
				.SetAdvanceFeed(STATE_ADVANCE)
				.SetStateMachine(_fsm);
		} else
		{
			state = new RootSinkState<string>(transform)
				.SetAnimator(_animator)
				.SetRisenPos(transform.position.ZeroY() + Vector3.up * riseHeight)
				.SetSinkDuration(sinkRiseSpeed)
				.SetAdvanceFeed(STATE_ADVANCE)
				.SetStateMachine(_fsm);
		}
		return state;
	}

	private RootAttackState<string> CreateAttackState(int attackId) {
		var state = new RootAttackState<string>(transform)
			.SetAdvanceFeed(STATE_ADVANCE)
			.SetAnimator(_animator)
			.SetAttackId(attackId)
			.SetDamagingLayers(components.AttackMask)
			.SetOwner(boss).SetHitbox(hitboxes[attackId])
			.SetTimeUntillAttack(attackDelays[attackId]);

		state.SetStateMachine(_fsm);
		return state;
	}



	public void OnAnimationEnd() {
		throw new System.NotImplementedException();
	}

	public GameObject SourceObject => gameObject;

	public Team GetTeam => components.Team;

	public bool Damage(int amount, IDamager source) => boss.Damage(amount, source);

	public void Die(IDamager source) => _fsm.Feed(STATE_DIE);

	public event Action<IDamager> OnDeath;

#if UNITY_EDITOR
	private void OnDrawGizmosSelected() {
		int c = hitboxes.Count;
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;

		Gizmos.color = Color.white;
		CheckAndShowGizmo(0);

		Gizmos.color = Color.blue;
		CheckAndShowGizmo(1);

		Gizmos.color = Color.red;
		CheckAndShowGizmo(2);

		Gizmos.color = Color.green;
		CheckAndShowGizmo(3);

		void CheckAndShowGizmo(int i) {
			if(c > i)
				hitboxes[i].DrawGizmo(pos, rot);
		}
	}
#endif

	public struct Placement {
		public Vector3 pos;
		public Quaternion rot;
	}
}