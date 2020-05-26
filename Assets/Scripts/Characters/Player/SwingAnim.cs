using UnityEngine;

public class SwingAnim : MonoBehaviour {
	public string enterParameter;
	public string swingValueParameter;
	public string exitParameter;

	private Animator animator;

	private void Start() => 
		animator = GetComponent<Animator>();

	public void BeginSwing() => 
		animator.SetTrigger(enterParameter);

	public void EndSwing() =>
		animator.SetTrigger(enterParameter);

	public void UpdateStatus(float angle) => 
		animator.SetFloat(swingValueParameter, 1 - (angle / 90));
}
