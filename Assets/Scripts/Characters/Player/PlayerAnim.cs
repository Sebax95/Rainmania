using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

	private Player player;
	private Animator thisAnimator;
	public Vector2 directionAnimMultiplier;

	public string param_horizontalSpeed;
	public string param_verticalSpeed;

	public string[] param_triggers;
	public string[] param_bools;

	private void Awake() {
		thisAnimator = GetComponent<Animator>();
		player = GetComponent<Player>();
	}

	private void Update() {
		DetectGround();
	}

	public void SetSpeeds(Vector2 speeds) {
		if(speeds.x > 0)
			transform.rotation = Quaternion.Euler(0, 90, 0);
		else if(speeds.x < 0)
			transform.rotation = Quaternion.Euler(0, 270, 0);
		thisAnimator.SetFloat(param_horizontalSpeed, Mathf.Abs(speeds.x * directionAnimMultiplier.x));
		thisAnimator.SetFloat(param_verticalSpeed, Mathf.Abs(speeds.y * directionAnimMultiplier.y));

	}

	/// <summary>
	/// 0: Jump
	/// </summary>
	/// <param name="index"></param>
	public void TriggerAction(int index) {
		thisAnimator.SetTrigger(param_triggers[index]);
	}

	public void ChangeBool(int index, bool value) =>
		thisAnimator.SetBool(param_bools[index], value);

	private void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer == 9)
			ChangeBool(0,true);
		
	}

	public void DetectGround() {
		ChangeBool(0, player.Grounded);
	}
}
