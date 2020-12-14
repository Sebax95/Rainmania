using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class PlayerAnim : TimedBehaviour {

	[Header("Animation")]
	private Animator thisAnimator;
	public Vector2 directionAnimMultiplier;
	private Player player;

	public GameObject whipRig;
	public GameObject whip;
	public GameObject bow;
	public Transform targetHead;
	public Rig headRig;
	public Transform[] anchorPos;
	
	[Header("Animation parameters")] 
	public string param_horizontalSpeed;
	public string param_verticalSpeed;
	public string param_horizontalAim;
	public string param_verticalAim;

	public string[] param_triggers;
	public string[] param_bools;

	private UIManager UI;

	[Header("Audio")]
	public AudioClip hurtSound;
	public AudioClip jumpSound;
	public AudioClip whipSound;
	public AudioClip bowSound;
	public AudioClip dieSound;
	public AudioClip[] stepSoundsGround;
	public AudioClip[] stepSoundsWater;

	private AudioSource _audioSource;
	
	private AudioClip RandomGroundStep => stepSoundsGround[Random.Range(0, stepSoundsGround.Length)];
	private AudioClip RandomWaterStep => stepSoundsWater[Random.Range(0, stepSoundsWater.Length)];
	public bool InWater { get; set; }

	private enum AudioCue {
		Hurt, Jump, Whip, Bow, Step, Die
	}

	private void Awake() {
		thisAnimator = GetComponent<Animator>();
		player = GetComponent<Player>();
		_audioSource = GetComponent<AudioSource>();
		whipRig.SetActive(false);
		whip.SetActive(false);
		bow.SetActive(false);
	}

	private void Start() {
		UI = Fetchable.FetchComponent<UIManager>("PlayerHUD");
	}

	protected override void OnUpdate(){
		DetectGround();
	}

	public void SetSpeeds(Vector2 speeds) {
		thisAnimator.SetFloat(param_horizontalSpeed, Mathf.Abs(speeds.x * directionAnimMultiplier.x));
		thisAnimator.SetFloat(param_verticalSpeed, speeds.y * directionAnimMultiplier.y);
		//HeadFollower(speeds);
	}

	public void SetAims(Vector2 aims)
	{
		thisAnimator.SetFloat(param_horizontalAim, Mathf.Abs(aims.x * directionAnimMultiplier.x));
		thisAnimator.SetFloat(param_verticalAim, aims.y * directionAnimMultiplier.y);
	}

	public void HeadFollower(Vector2 position)
	{
		var selected = anchorPos[0].position;
		if(position == new Vector2(0,1))
			selected = anchorPos[2].position;
		else if(position == new Vector2(1,1) || position == new Vector2(-1,1))
			selected = anchorPos[1].position;
		else
			selected = anchorPos[0].position;
		targetHead.position = selected;
		//StartCoroutine(LerpMovement(selected));
	}

	//Temp es lo del rigging bug
	public void DeActivateHeadFollower() => headRig.weight = 0;
	public void ActivateHeadFollower() => headRig.weight = 0.8f;
	
	//TODO: fixear esto para que la transicion de donde mire sea smooth
	IEnumerator LerpMovement(Vector2 pos)
	{
		var initTimer = 0f;
		var finalTimer = 1f;
		var startPos = pos;
		while (initTimer < finalTimer)
		{
			targetHead.position = Vector3.Lerp(targetHead.position, startPos, (initTimer / finalTimer));
			initTimer += Time.deltaTime;
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	/// <summary>
	/// 0: Jump
	/// 1: Hurt
	/// 2: Whip
	/// 3: Bow
	/// </summary>
	/// <param name="index"></param>
	public void TriggerAction(int index) => 
		thisAnimator.SetTrigger(param_triggers[index]);

	/// <summary>
	/// 0: inGround
	/// 1: isDead
	/// 2: isCrounched
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
		/*if(vertical)
			if(horizontal)
				Activate(2, 1); //Diagonal
			else
				Activate(3, 2); //Vertical
		else
			Activate(1, 0); //Horizontal
		void Activate(int animIndex, int waiterIndex) {
			TriggerAction(animIndex);
		}*/
		TriggerAction(3);
		PlaySound(AudioCue.Bow);
	}

	public void WhipAttack(Vector2 direction) {
		bool vertical = direction.y > 0;
		bool horizontal = direction.x != 0;
		whip.SetActive(true);
		TriggerAction(2);
		/*if(vertical)
			if(horizontal)
				Activate(5);
			else
				Activate(6);
		else
			Activate(4);

		void Activate(int animIndex) {
			TriggerAction(animIndex);
		}*/
		PlaySound(AudioCue.Whip);
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
		PlaySound(AudioCue.Jump);
	}

	public void Hurt() {
		TriggerAction(1);
		UI.SetHealthbarPercent(player.Health / player.maxHealth);
		PlaySound(AudioCue.Hurt);
	}

	public void Heal() {
		UI.SetHealthbarPercent(player.Health / player.maxHealth);
	}

	public void Die() {
		DeActivateHeadFollower();
		ChangeBool(1, true);
		PlaySound(AudioCue.Die);
	}

	public void SetCrouched(bool state) {
		ChangeBool(2, state);
	}

	private void PlaySound(AudioCue sound, float volume = 1) {
		AudioClip selected = null;

		switch(sound)
		{
			case AudioCue.Hurt:
				selected = hurtSound;
				break;
			case AudioCue.Jump:
				selected = jumpSound;
				break;
			case AudioCue.Whip:
				selected = whipSound;
				break;
			case AudioCue.Bow:
				selected = bowSound;
				break;
			case AudioCue.Step:
				selected = InWater ? RandomWaterStep : RandomGroundStep;
				break;
			case AudioCue.Die:
				selected = dieSound;
				break;
		}
		if(selected)
			_audioSource.PlayOneShot(selected,volume);
	}

	//Eventos de animation
	public void BowOn() => bow.SetActive(true);
	public void BowOff() => bow.SetActive(false);

	public void WhipOn()
	{
		whipRig.SetActive(true);
		whip.SetActive(true);
	}

	public void Step(float volume) => PlaySound(AudioCue.Step, volume);

	public void WhipOff()
	{
		whipRig.SetActive(false);
		whip.SetActive(false);
	}

	private void OnEnable() => ActivateHeadFollower();

	private void OnDisable() => DeActivateHeadFollower();
}
