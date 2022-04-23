using UnityEngine;

public class AnimationSpeedChanger : MonoBehaviour {

	private Animator animator;

	private void Awake() => animator = GetComponent<Animator>();

	public void SetAnimSpeed(float mult) {
		animator.SetFloat("AnimMult", mult);
	}
}
