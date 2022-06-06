using UnityEngine;
using BossMachine;

public class RootSinkState<T> : FiniteState<T> {
	private Vector3 sinkPosition;
	private Vector3 risePosition;
	private float lerpPos;
	private float riseDuration;

	private Animator animator;
	private T advanceFeed;

	public RootSinkState(Transform transform) : base(transform) { }

	public override void Enter() {
		base.Enter();
		risePosition = transform.position;
		lerpPos = 1f;
		animator.SetBool("Risen", false);
	}
	public override void Update() {
		base.Update();
		lerpPos = lerpPos - (Time.deltaTime / riseDuration);
		transform.position = Vector3.Lerp(sinkPosition, risePosition, lerpPos);
		if(lerpPos <= 0f)
			fsm.Feed(advanceFeed);
	}

	#region Builder
	public RootSinkState<T> SetRisenPos(Vector3 position) {
		risePosition = position;
		return this;
	}
	public RootSinkState<T> SetSinkDuration(float time) {
		riseDuration = time;
		return this;
	}
	public RootSinkState<T> SetAnimator(Animator animator) {
		this.animator = animator;
		return this;
	}
	public RootSinkState<T> SetAdvanceFeed(T feed) {
		advanceFeed = feed;
		return this;
	}

	#endregion

}
