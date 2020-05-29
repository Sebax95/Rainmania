using UnityEngine;

public class SwingAnim : MonoBehaviour {
	public string swingingParameter;
	public string swingValueParameter;
	public string wrappedSwingingParameter;

	private Animator animator;

	private void Start() =>
		animator = GetComponent<Animator>();

	public void BeginSwing() =>
		animator.SetBool(swingingParameter, true);

	public void EndSwing() =>
		animator.SetBool(swingingParameter, true);

	public void UpdateStatus(float angle) {
		float mod = 0;
		float mult = 1;
		if(transform.forward.x < 0)
		{
			mod = 2;
			mult = -1;
		}
		float value = mod + mult * 1 + (angle / Swinger.HALF_PI);
		animator.SetFloat(swingValueParameter, value);
		animator.SetFloat(wrappedSwingingParameter, value % 1);
	}
}
