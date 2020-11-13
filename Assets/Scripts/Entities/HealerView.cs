using UnityEngine;

public class HealerView : TimedBehaviour {

	public float spinSpeed;

	private Transform child;
	private Vector3 axis;

	public bool Active { get; set; }


	private void Start() {
		child = transform.GetChild(0);
		axis = transform.up;
		Active = true;
	}

	protected override void OnUpdate(){
		if(!Active)
			return;
		child.Rotate(axis, spinSpeed * Time.deltaTime);
	}

	public void Pickup() {
		Active = false;

	}

	public void Respawn() {
		Active = true;

	}
}
