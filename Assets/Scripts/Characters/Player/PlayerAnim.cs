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

	private WaitForSeconds[] wait_whipAnimDuration = new WaitForSeconds[] {
		//normal atck, , up,diagonal
		new WaitForSeconds(0.3f), new WaitForSeconds(0.65f), new WaitForSeconds(0.15f)

	};
	private WaitForSeconds[] wait_bowAnimDuration = new WaitForSeconds[] {
		new WaitForSeconds(0.3f), new WaitForSeconds(0.3f), new WaitForSeconds(0.3f)
	};

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

	public void Attack(Vector2 direction, string name) {
		switch(name)
		{
			case Whip.NAME:
				WhipAttack(direction);
				break;
			case Bow.NAME:
				BowAttack(direction);
				break;
			default:
				Debug.LogWarning("Missing animation for weapon " + name);
				break;
		}
	}

	public void BowAttack(Vector2 direction) {
		bool vertical = direction.y > 0;
		bool horizontal = direction.x != 0;
		bow.SetActive(true);
		if(vertical)
			if(horizontal)
				Activate(2, 1);
			else
				Activate(3, 2);
		else
			Activate(1, 0);
		
		void Activate(int animIndex, int waiterIndex) {
			TriggerAction(animIndex);
			StartCoroutine(Coroutine_DelayedHiding(bow, wait_bowAnimDuration[waiterIndex]));
		}
	}

	public void WhipAttack(Vector2 direction) {
		bool vertical = direction.y > 0;
		bool horizontal = direction.x != 0;
		whip.SetActive(true);
		if(vertical)
			if(horizontal)
				Activate(5, 1);
			else
				Activate(6, 1);
		else
			Activate(4, 0);

		void Activate(int animIndex, int waiterIndex) {
			TriggerAction(animIndex);
			StartCoroutine(Coroutine_DelayedHiding(whip, wait_whipAnimDuration[waiterIndex]));
		}
	}

	public void WeaponsActive() {
		whip.SetActive(false);
		bow.SetActive(false);
	}

	private void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer == 9)
			ChangeBool(0, true);
	}

	public void DetectGround() {
		ChangeBool(0, player.Grounded);
	}

	public void Jump() {
		TriggerAction(0);
		ChangeBool(0, false);
	}

	private IEnumerator Coroutine_DelayedHiding(GameObject item, WaitForSeconds waiter) {
		item.SetActive(true);
		yield return waiter;
		item.SetActive(false);
	}
}
