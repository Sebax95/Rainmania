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
	private RigidbodyConstraints constraints;
	private int originalLayer;

	private void Awake() {
		rigid = GetComponent<Rigidbody>();
		constraints = rigid.constraints;
		originalLayer = gameObject.layer;
	}

	void Update() {
		if(!stop)
			rigid.velocity = transform.forward * speed;
		else
		{
			timer += Time.deltaTime;

			if(isStair && timer > asPlatformLifetime)
				shooter.ReturnArrow(this);
			else if(!isStair && timer > droppedArrowLifetime)
				shooter.ReturnArrow(this);
		}
		if(timer > asPlatformLifetime)
			shooter.ReturnArrow(this);
	}

	public void Reset() {
		stop = false;
		timer = 0;
		rigid.isKinematic = false;
		rigid.constraints = constraints;
		rigid.velocity = transform.forward * speed;
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
		if(!collision.collider.gameObject || gameObject.layer == 1)
			return;

		stop = true;
		if(gameObject.layer == 9 || gameObject.layer == 1)
			return;

		if(collision.collider.gameObject.layer == 9 && !collision.collider.gameObject.CompareTag("UnsusedArrow"))
		{
			rigid.isKinematic = true;
			rigid.velocity = Vector3.zero;
			gameObject.tag = "UnsusedArrow";
			isStair = true;
		} else
		{
			this.gameObject.layer = 11;
			rigid.useGravity = true;
			rigid.constraints = (RigidbodyConstraints)104; //8 + 32 + 64. 8 = Freeze Z position. 32 & 64 = Freeze YZ rotation.
			//rigid.constraints = (RigidbodyConstraints)56; //8 + 16 + 32. 8 = Freeze Z position. 16 & 32 = Freeze XY rotation.
			//rigid.constraints = (RigidbodyConstraints)72; //8 + 64. 8 = Freeze Z position. 64 = Freeze Z rotation.

		}

	}
}