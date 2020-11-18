using UnityEngine;

public abstract class TimedBehaviour : MonoBehaviour {
	protected static bool RunUpdates { get; set; } = true;
	public const float DEFAULT_TIMESCALE = 1;

	void Update() {
		if(RunUpdates)
			OnUpdate();
	}
	private void FixedUpdate() {
		if(RunUpdates)
			OnFixedUpdate();
	}
	private void LateUpdate() {
		if(RunUpdates)
			OnLateUpdate();
	}

	protected virtual void OnUpdate() { }
	protected virtual void OnFixedUpdate() { }
	protected virtual void OnLateUpdate() { }

	public static void SetPause(bool isPaused) {
		RunUpdates = !isPaused;
		Time.timeScale = isPaused ? 0 : DEFAULT_TIMESCALE;
	}
}
