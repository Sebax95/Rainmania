using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

	private Player player;
	private Animator thisAnimator;
	public Vector2 directionAnimMultiplier;

	public GameObject whip;
	public GameObject bow;

	public string param_horizontalSpeed;
	public string param_verticalSpeed;

	public string[] param_triggers;
	public string[] param_bools;

	private void Awake() {
		thisAnimator = GetComponent<Animator>();
		player = GetComponent<Player>();
		whip.SetActive(false);
		bow.SetActive(false);
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

	public void BowAttackAnim(int BNormal, int BDiag, int BUP, Vector2 direction)
	{
		bool vertical = direction.y > 0;
		bool horizontal = direction.x != 0;
		bow.SetActive(true);
		if (vertical)
			if (horizontal)
				thisAnimator.SetTrigger(param_triggers[BDiag]);
			else
				thisAnimator.SetTrigger(param_triggers[BUP]);
		else
			thisAnimator.SetTrigger(param_triggers[BNormal]);
	}
	public void WipAttackAnim(int WNormal, int WDiag, int WUP, Vector2 direction)
	{
		bool vertical = direction.y > 0;
		bool horizontal = direction.x != 0;
		whip.SetActive(true);
		if (vertical)
			if (horizontal)
				thisAnimator.SetTrigger(param_triggers[WDiag]);
			else
				thisAnimator.SetTrigger(param_triggers[WUP]);
		else
			thisAnimator.SetTrigger(param_triggers[WNormal]);
	}

	public void WeaponsActive()
	{
		whip.SetActive(false);
		bow.SetActive(false);
	}

	private void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer == 9)
			ChangeBool(0,true);
		
	}

	public void DetectGround() {
		ChangeBool(0, player.Grounded);
	}
}
