using System.Collections.Generic;
using UnityEngine;
using BossMachine;

public class MushroomBoss : MonoBehaviour {

	private StateMachine<string> fsm;
	private Transform player;
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private List<BossRoot> roots;

	// Start is called before the first frame update
	void Start() {
		player = Fetchable.FetchRaw("player").transform;
		InitializeStateMachine();
	}

	// Update is called once per frame
	void Update() {

	}

	private void InitializeStateMachine() {
		fsm = new StateMachine<string>();

	}
}