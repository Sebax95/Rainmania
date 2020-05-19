using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	public float speed;
	private bool stop = false;
	private Rigidbody rigid;
	public float asPlatformLifetime = 3;
	public float droppedArrowLifetime = 1;
	public float maxFlightTime = 5;
	private float timer;
	private bool isStair;
	private Bow shooter;
	private int originalLayer;
	

	private const int FLIGHT_CONSTRAINTS = 104;//8 + 32 + 64. 8 = Freeze Z position. 32 & 64 = Freeze YZ rotation.

	private void Awake() {
		rigid = GetComponent<Rigidbody>();
		originalLayer = gameObject.layer;
	}

	void Update() {

		if(!stop)
			rigid.velocity = transform.forward * speed;

		ExpirationTimers();

	}

	private void ExpirationTimers() {
		timer += Time.deltaTime;
		if(!isStair)
		{
			if((!stop && timer > maxFlightTime) || (stop && timer > droppedArrowLifetime)) //If flying and expire, or dropped and expire
				shooter.ReturnArrow(this); //Return to pool
			return;
		} 
		if(timer > asPlatformLifetime) //If platform and expire
			shooter.ReturnArrow(this); //Return to pool
	}

	public void Reset() {
		stop = false;
		isStair = false;
		timer = 0;
		rigid.isKinematic = false;
		rigid.velocity = transform.forward * speed;
		rigid.constraints = (RigidbodyConstraints)FLIGHT_CONSTRAINTS;
		this.gameObject.tag = "Arrow";
		gameObject.layer = originalLayer;
		rigid.useGravity = false;

	}
	public static void TurnOn(Arrow a) {
		a.Reset();
		a.gameObject.SetActive(true);
	}

	public static void TurnOff(Arrow a) {
		a.gameObject.SetActive(false);
	}

	public void SetShooter(Bow source) => shooter = source;

	private void OnCollisionEnter(Collision collision) {
		if(!enabled) //Por alguna razon, este metodo se activa aun cuando esta desactivado
			return;

		if(isStair) //Es plataforma, no cambies nada
			return;

		if(!collision.collider.gameObject || gameObject.layer == 1)
			return;

		stop = true;
		timer = 0;
		if(gameObject.layer == 14 || gameObject.layer == 1)
			return;

		if(collision.collider.gameObject.layer == 14 && !collision.collider.gameObject.CompareTag("UnsusedArrow"))
		{
			rigid.isKinematic = true;
			rigid.velocity = Vector3.zero;
			gameObject.tag = "UnsusedArrow";
			isStair = true;
		} else
		{
			this.gameObject.layer = 11;
			rigid.useGravity = true;
			rigid.constraints = (RigidbodyConstraints)FLIGHT_CONSTRAINTS;

		}

	}
}