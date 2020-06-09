using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IDamager {

	public float speed;

	[Header("Tiempos")]
	public float asPlatformLifetime = 3;
	public float droppedArrowLifetime = 1;
	public float maxFlightTime = 5;
	public float grappleTime = 10;
	private float timer;

	//Tags
	private const string BAD_ARROW_TAG = "UnsusedArrow";
	private const string ANCHORABLE_TAG = "Roof";

	//State
	//private bool isAnchor;
	//private bool isStair;
	private int originalLayer;
	private bool stop = false;

	//References
	private Bow shooter;
	private Rigidbody rigid;
	private SwingAnchor anchor;
    private TrailRenderer trail;


	private const int FLIGHT_CONSTRAINTS = 104;//8 + 32 + 64. 8 = Freeze Z position. 32 & 64 = Freeze YZ rotation.

	private void Awake() {
        trail = GetComponentInChildren<TrailRenderer>();
		rigid = GetComponent<Rigidbody>();
		anchor = GetComponent<SwingAnchor>();
		originalLayer = gameObject.layer;
	}

	void Update() {

		if(!stop)
			rigid.velocity = transform.forward * speed;

		ExpirationTimers();

	}

	//TODO: set timer to countDOWN, set timer time on corresponding event, then simply check to 0
	private void ExpirationTimers() {
		timer -= Time.deltaTime;

		if(timer < 0)
			shooter.ReturnArrow(this); //Return to pool
	}

	public void Reset() {
		stop = false;
		//isStair = false;
		//isAnchor = false;
		timer = maxFlightTime;
        trail.enabled = true;
		rigid.isKinematic = false;
		rigid.velocity = transform.forward * speed;
		rigid.constraints = (RigidbodyConstraints)FLIGHT_CONSTRAINTS;
		this.gameObject.tag = "Arrow";
		gameObject.layer = originalLayer;
		rigid.useGravity = false;
		anchor.enabled = false;

	}
	public static void TurnOn(Arrow a) {
		a.Reset();
		a.gameObject.SetActive(true);
	}

	public static void TurnOff(Arrow a) {
		a.gameObject.SetActive(false);
	}

	public void SetShooter(Bow source) => shooter = source;

	protected virtual void OnCollisionEnter(Collision collision) {
		if(!enabled) //Por alguna razon, este metodo se activa aun cuando esta desactivado
			return;

		if(stop) //Si no se mueve, no necesita interacciones
			return;

		var dmg = collision.collider.GetComponent<IDamageable>();
		if(dmg != null)
			dmg.Damage(shooter.damage, this);


		if(!collision.collider.gameObject || gameObject.layer == 1)
			return;

		stop = true;
        trail.enabled = false;
        //timer = 0;
        if (gameObject.layer == 14 || gameObject.layer == 1)
			return;

		if(collision.collider.gameObject.layer == 14 && !collision.collider.gameObject.CompareTag(BAD_ARROW_TAG))
		{
			rigid.isKinematic = true;
			rigid.velocity = Vector3.zero;
			gameObject.tag = BAD_ARROW_TAG;
			//isStair = true;
			timer = asPlatformLifetime;
			if(collision.collider.gameObject.tag == ANCHORABLE_TAG)
				anchor.enabled = true;
		} else
		{
			this.gameObject.layer = 11;
			rigid.useGravity = true;
			rigid.constraints = (RigidbodyConstraints)FLIGHT_CONSTRAINTS;
			timer = droppedArrowLifetime;
		}

	}

	public void OnGrapple() {
		//isAnchor = true;
		timer = grappleTime;
	}

	//IDamager
	public GameObject SourceObject => shooter.gameObject;
	public Team GetTeam => shooter.GetTeam;
}