using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Arroww = Arrow;
using Arrow = testing.Arrow;

namespace testing {
	public class Arrow : MonoBehaviour {

		public float speed;
		bool stop;
		Rigidbody rigid;
		float timer;
		public float lifeTime;
		bool stuck = false;
		bool noStuck = false;
		//bool isStair;
		private Bow shooter;

		private void Awake() {
			rigid = GetComponent<Rigidbody>();
			stuck = false;
			noStuck = false;
			this.gameObject.tag = "Arrow";
		}
		void Update() {
			if(!stop)
				transform.position += transform.forward * speed * Time.deltaTime;
			else
			{
				timer += 1 * Time.deltaTime;
				//if(isStair)
				//{
				//	if(timer > lifeTime)
				//		shooter.ReturnArrow((Arroww)this);
				//} else
				//{
				//	if(timer > 1)
				//		shooter.ReturnArrow((Arroww)this);
				//}



			}
		}
		public void Reset() {
			gameObject.layer = 10;
			this.gameObject.tag = "Arrow";
			stop = false;
			timer = 0;
			if(stuck)
			{
				stuck = false;
				rigid = this.gameObject.AddComponent<Rigidbody>();
				rigid.constraints = RigidbodyConstraints.FreezeAll;
			}
			if(noStuck)
			{
				noStuck = false;
				rigid.constraints = RigidbodyConstraints.None;
				rigid.constraints = RigidbodyConstraints.FreezeAll;
			}
		}
		public static void TurnOn(Arrow a) {
			a.Reset();
			a.gameObject.SetActive(true);
		}

		public static void TurnOff(Arrow a) {

			a.gameObject.SetActive(false);
		}

		private void OnCollisionEnter(Collision collision) {
			if(!collision.collider.gameObject)
				return;

			stop = true;

			if(gameObject.layer == 1)
				return;
			if(collision.collider.gameObject.layer == 9 && this.gameObject.layer != 9  && !collision.collider.gameObject.CompareTag("UnsusedArrow"))
			{
				this.gameObject.layer = 9;
				Destroy(GetComponent<Rigidbody>());
				this.gameObject.tag = "UnsusedArrow";
				stuck = true;
				//isStair = true;
			} else if(this.gameObject.layer != 9)
			{
				this.gameObject.layer = 0;
				rigid.constraints = RigidbodyConstraints.None;
				rigid.constraints = RigidbodyConstraints.FreezePositionZ;
				rigid.constraints = RigidbodyConstraints.FreezeRotationY;
				noStuck = true;
				//isStair = false;
			}
		}

		public void SetShooter(Bow source) => shooter = source;
	}
}