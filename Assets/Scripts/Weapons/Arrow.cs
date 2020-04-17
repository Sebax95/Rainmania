using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	public float speed;
	private bool stop = false;
	private Rigidbody rigid;
	public float lifetime = 3;
	private float timer;
	private bool secondTime = false;
	private Bow shooter;
	private RigidbodyConstraints constraints;

	private void Awake() {
		rigid = GetComponent<Rigidbody>();
		constraints = rigid.constraints;
	}

	private void Start() {
	}

	void Update() {
		if(!stop)
			rigid.velocity = transform.forward * speed;
		else
			timer += Time.deltaTime;
		if(timer > lifetime)
			shooter.ReturnArrow(this);
	}

	public void Reset() {
		stop = false;
		timer = 0;
		rigid.isKinematic = false;
		rigid.constraints = constraints;
		rigid.velocity = transform.forward * speed;

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
		if(!collision.collider.gameObject)
			return;

		secondTime = true;
		stop = true;
		if(gameObject.layer == 9 || gameObject.layer == 1)
			return;
		if(collision.collider.gameObject.layer == 9)
		{
			rigid.isKinematic = true;
			rigid.velocity = Vector3.zero;
		} else
		{
			this.gameObject.layer = 1;
			rigid.constraints = constraints;

		}

	}
}