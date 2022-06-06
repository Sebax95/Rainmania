using System.Collections.Generic;
using UnityEngine;
using BossMachine;
using System;

public class MushroomBoss : MonoBehaviour, IAnimationEndActivable, IDamager, IDamageable {

	public const string STATE_TIMED_END = "StateTimeEnd";
	public const string STATE_HP_GATE_REACHED = "StateHpGate";
	public const string STATE_DIE = "StateDie";

	private StateMachine<string> fsm;

	[SerializeField]
	private int maxHp = 10;
	private int currentHp;
	private bool isImmune = false;

	[SerializeField]
	private List<int> damageGates;
	private Queue<int> damageGateQueue;

	[SerializeField]
	private BossComponents components = new BossComponents();


	// Start is called before the first frame update
	void Start() {
		components.Initialize();
		InitializeStateMachine();
		InitializeRoots();
		damageGateQueue = new Queue<int>(damageGates);
		currentHp = maxHp;
	}

	private void InitializeRoots() {
		var roots = components.Roots;
		foreach(var item in roots)
			item.components = components;
	}

	// Update is called once per frame
	void Update() {
		fsm.Update();
	}

	private void InitializeStateMachine() {
		fsm = new StateMachine<string>();

	}

	public void OnAnimationEnd() {
		fsm.Feed(STATE_TIMED_END);
	}

	public GameObject SourceObject => gameObject;

	public Team GetTeam => components.Team;

	public bool Damage(int amount, IDamager source) {
		if(isImmune || currentHp <= 0)
			return false;

		currentHp -= amount;
		if(currentHp < damageGateQueue.Peek())
		{
			fsm.Feed(STATE_HP_GATE_REACHED);
		}
		if(currentHp <= 0)
			Die(source);
		return true;
	}

	public void Die(IDamager source) {
		isImmune = true;
		fsm.Feed(STATE_DIE);
	}

	public event Action<IDamager> OnDeath;

	[Serializable]
	public class BossComponents {
		private Transform _player;
		[SerializeField] private Transform _transform;
		[SerializeField] private Animator _animator;
		[SerializeField] private List<BossRoot> _roots;
		[SerializeField] private Team _team;
		[SerializeField] private LayerMask _attackLayer;

		public Transform transform => _transform;
		public Transform Player => _player;
		public Animator Animator => _animator;
		public List<BossRoot> Roots => _roots;
		public Team Team => _team;
		public LayerMask AttackMask => _attackLayer;

		public void Initialize() {
			_player = Fetchable.FetchRaw("player").transform;
		}
	}
}