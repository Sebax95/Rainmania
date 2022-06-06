using System;
using UnityEngine;
using BossMachine;

public class RootRiseState<T> : FiniteState<T> {
	private Vector3 sinkPosition;
	private Vector3 risePosition;
	private float lerpPos;
	private float riseDuration;

	private Animator animator;
	private T advanceFeed;

	public RootRiseState(Transform transform) : base(transform) { }

	public override void Enter() {
		base.Enter();
		sinkPosition = transform.position;
		lerpPos = 0f;
		animator.SetBool("Risen", true);
	}
	public override void Update() {
		base.Update();
		lerpPos = lerpPos + (Time.deltaTime / riseDuration);
		transform.position = Vector3.Lerp(sinkPosition, risePosition, lerpPos);
		if(lerpPos >= 1f)
			fsm.Feed(advanceFeed);
	}

	#region Builder
	public RootRiseState<T> SetRisePos(Vector3 position) {
		risePosition = position;
		return this;
	}
	public RootRiseState<T> SetRiseDuration(float time) {
		riseDuration = time;
		return this;
	}
	public RootRiseState<T> SetAnimator(Animator animator) {
		this.animator = animator;
		return this;
	}
	public RootRiseState<T> SetAdvanceFeed(T feed) {
		advanceFeed = feed;
		return this;
	}

	#endregion
}