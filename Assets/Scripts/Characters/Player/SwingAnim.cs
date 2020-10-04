using UnityEngine;

public class SwingAnim : MonoBehaviour {
	public string swingingParameter;
	public string swingValueParameter;
	public string wrappedSwingingParameter;

	private Animator animator;
	private float lastAngle;

	private void Start() =>
		animator = GetComponent<Animator>();

	public void BeginSwing() =>
		animator.SetBool(swingingParameter, true);

	public void EndSwing() =>
		animator.SetBool(swingingParameter, false);

	public void UpdateStatus(float angle) {
		lastAngle = angle;
		float mod = 0;
		float mult = 0.5f;
		if(transform.forward.x < 0)
		{
			mod = 1;
			mult = -0.5f;
		}
		float value = Mathf.Clamp01(mod + mult * (1 + (angle / Swinger.HALF_PI)));
		animator.SetFloat(swingValueParameter, value);
		animator.SetFloat(wrappedSwingingParameter, value);
	}

	public void UpdateWithLast() => UpdateStatus(lastAngle);
}
