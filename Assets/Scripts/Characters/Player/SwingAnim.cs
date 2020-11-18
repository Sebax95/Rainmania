using UnityEngine;

public class SwingAnim : TimedBehaviour {
	public string swingingParameter;
	public string swingValueParameter;
	public string wrappedSwingingParameter;

	private Animator animator;
	private float lastAngle;

	//Agus paso por aqui
	public GameObject brilloPrefab;
	GameObject brillo;

	private void Start()
    {
		animator = GetComponent<Animator>();
		brillo = Instantiate(brilloPrefab);
		brillo.transform.position = new Vector3(00,0, 0);
    }

	public void BeginSwing(Transform _trans)
    {
		brillo.transform.position = _trans.position;
		brillo.SetActive(false);
		brillo.SetActive(true);
		animator.SetBool(swingingParameter, true);

    }

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
