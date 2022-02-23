using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arrow : TimedBehaviour, IDamager {

	public float speed;

	[Header("Tiempos")]
	public float asPlatformLifetime = 3;
	public float droppedArrowLifetime = 1;
	public float maxFlightTime = 5;
	public float grappleTime = 10;
	private float timer;

	[Header("Propiedades")]
	public bool canPlatform = true;
	public bool canAnchor = true;

	//Tags
	private const string BAD_ARROW_TAG = "UnsusedArrow";
	private const string ANCHORABLE_TAG = "ArrowStick";

	//State
	private int originalLayer;
	private bool stop = false;

	//References
	private Bow shooter;
	private Rigidbody rigid;
	private Collider solidCol;
	private Collider triggerCol;
	private SwingAnchor anchor;
	private TrailRenderer trail;


	private const int FLIGHT_CONSTRAINTS = 104;//8 + 32 + 64. 8 = Freeze Z position. 32 & 64 = Freeze YZ rotation.
	public const int WALL_LAYER = 14;
	private const int PLAYER_LAYER = 8;

	//Agus paso por aqui
	public ParticleSystem wallInpact;
	public ParticleSystem metalInpact;

	private void Awake() {
		trail = GetComponentInChildren<TrailRenderer>();
		rigid = GetComponent<Rigidbody>();
		anchor = GetComponent<SwingAnchor>();
		originalLayer = gameObject.layer;
		var cols = GetComponents<Collider>();
		solidCol = cols[0];
		triggerCol = cols[1];
	}

	private void OnEnable() {
		anchor.enabled = canAnchor;
	}

	protected override void OnUpdate() {
		if(!stop)
			rigid.velocity = transform.forward * speed;

		ExpirationTimers();
	}

	private void ExpirationTimers() {
		timer -= Time.deltaTime;

		if(timer < 0)
			gameObject.SetActive(false);
		//shooter.ReturnArrow(this); //Return to pool
	}

	public void Reset() {
		stop = false;
		//isStair = false;
		//isAnchor = false;
		timer = maxFlightTime;
		rigid.isKinematic = false;
		rigid.velocity = transform.forward * speed;
		rigid.constraints = (RigidbodyConstraints)FLIGHT_CONSTRAINTS;
		this.gameObject.tag = "Arrow";
		gameObject.layer = originalLayer;
		rigid.useGravity = false;
		anchor.enabled = false;
		solidCol.enabled = true;

		SetIsFlying(true);

	}
	public static void TurnOn(Arrow a) {
		a.Reset();
		a.gameObject.SetActive(true);
		//UIManager.Instance.ArrowAmount(-1);
	}

	public static void TurnOff(Arrow a) {
		a.gameObject.SetActive(false);
		//UIManager.Instance.ArrowAmount(+1);
	}

	public void SetShooter(Bow source) => shooter = source;

	protected virtual void OnCollisionEnter(Collision collision) {
		if(!enabled) //Por alguna razon, este metodo se activa aun cuando esta desactivado
			return;

		if(stop) //Si no se mueve, no necesita interacciones
			return;

		var dmg = collision.collider.GetComponent<IDamageable>();
		bool damaged = false;
		if(dmg != null)
		{
			if(dmg.SourceObject != shooter.SourceObject)
				damaged = dmg.Damage(shooter.damage, this);
		}

		SetIsFlying(false);

		//if(collision.collider.gameObject.layer == WALL_LAYER && !collision.collider.gameObject.CompareTag(BAD_ARROW_TAG))

		if(collision.collider.gameObject.CompareTag(ANCHORABLE_TAG) && canPlatform) //If it's a stickable wall
			StickToWall(collision);
		else if(damaged) //if it hit an enemy already, dissapear
			TurnOff(this);
		else //otherwise something non-stickable
			DropLimp(collision);


		if(collision.collider.gameObject.layer == 15) //Enemy layer
			gameObject.SetActive(false);
	}

	private void DropLimp(Collision collision) {
		if(collision.gameObject.CompareTag("Metal"))
			metalInpact.Play();
		gameObject.layer = 11; //Non-interactable layer
		rigid.useGravity = true;
		rigid.constraints = (RigidbodyConstraints)FLIGHT_CONSTRAINTS;
		timer = droppedArrowLifetime;
	}

	private void StickToWall(Collision collision) {
		rigid.isKinematic = true;
		gameObject.tag = BAD_ARROW_TAG;
		timer = asPlatformLifetime;
		//if(collision.collider.gameObject.tag == ANCHORABLE_TAG)
		anchor.enabled = canAnchor;

		Vector3 normal = -collision.GetContact(0).normal;
		if(Vector3.Angle(normal, transform.forward) > 50f)
			normal = transform.forward;
		transform.forward = normal;

		wallInpact.Play();

	}


	private void SetIsFlying(bool value) {
		stop = !value;
		triggerCol.enabled = !value;
		trail.enabled = value;
		rigid.velocity = Vector3.zero;
	}

	public void OnGrapple() {
		//isAnchor = true;
		timer = grappleTime;
	}

	public void LockExistance() {
		timer = float.PositiveInfinity;
	}

	public void UnlockExistance() {
		timer = grappleTime;
	}

	private void OnTriggerEnter(Collider other) {
		if(!stop || !canPlatform || other.gameObject.layer != PLAYER_LAYER)
			return;
		solidCol.enabled = false;
	}

	private void OnTriggerExit(Collider other) {
		solidCol.enabled = true;
	}

	//IDamager
	public GameObject SourceObject => shooter.gameObject;
	public Team GetTeam => shooter.GetTeam;
}