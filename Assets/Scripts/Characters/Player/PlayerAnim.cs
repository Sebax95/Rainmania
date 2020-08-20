using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public UIManager UI;

	private void Awake() {
		thisAnimator = GetComponent<Animator>();
		player = GetComponent<Player>();
		whip.SetActive(false);
		bow.SetActive(false);
	}

	private void Start() {
		UI = Fetchable.FetchComponent<UIManager>("PlayerHUD");
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
	/// 7: Hurt
	/// </summary>
	/// <param name="index"></param>
	public void TriggerAction(int index) {
		thisAnimator.SetTrigger(param_triggers[index]);
	}

	/// <summary>
	/// 1: Die
	/// </summary>
	/// <param name="index"></param>
	/// <param name="value"></param>
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
				Activate(2, 1); //Diagonal
			else
				Activate(3, 2); //Vertical
		else
			Activate(1, 0); //Horizontal
		
		void Activate(int animIndex, int waiterIndex) {
			TriggerAction(animIndex);
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

	public void Hurt() {
		TriggerAction(7);
		UI.SetHealthbarPercent(player.Health / player.maxHealth);
	}

	public void Heal() {
		UI.SetHealthbarPercent(player.Health / player.maxHealth);
	}

	public void Die() {
		ChangeBool(1, true);
	}

	//Eventos de animation
	public void BowOn() => bow.SetActive(true);
	public void BowOff() => bow.SetActive(false);

	public void WhipOn() => whip.SetActive(true);
	public void WhipOff() => whip.SetActive(false);
}
