using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {
	public Arrow arrownPrefab;
	private ObjectPool<Arrow> pool;
	private IWielder wielder;
	public Transform[] arrowSources;

	public override Team GetTeam => wielder.GetTeam;
	public override GameObject SourceObject => gameObject;

	private void Start() {
		wielder = GetComponent<IWielder>();
		pool = new ObjectPool<Arrow>(ArrownFactory, Arrow.TurnOn, Arrow.TurnOff, 5, true);
	}

	public Arrow ArrownFactory() => Instantiate(arrownPrefab);

	public void ReturnArrow(Arrow item) => pool.ReturnObject(item);

	public void BowNormal() {
		var arrow = pool.GetObject();
		arrow.SetShooter(this);
		arrow.transform.position = transform.position + arrowSources[0].position;
		arrow.transform.forward = arrowSources[0].forward;
	}
	
	public void BowDiag() {
		var arrow = pool.GetObject();
		arrow.SetShooter(this);
		arrow.transform.position = transform.position + arrowSources[1].position;
		arrow.transform.forward = transform.position + arrowSources[1].position;
	}

	public void BowUp() {
		var arrow = pool.GetObject();
		arrow.SetShooter(this);
		arrow.transform.position = transform.position + arrowSources[2].position;
		arrow.transform.forward = arrowSources[2].forward;
	}

	public override void Attack(Vector2 direction) {
		byte byteDirection = (byte)((direction.x == 0 ? 0 : 1 << 0) & (direction.y <= 0 ? 0 : 1 << 1));

		switch(byteDirection)
		{
			case 0: //No direction axis
				BowNormal();
				break;
			case 1: //Forward axis
				BowNormal();
				break;
			case 2: //Up axis
				BowUp();
				break;
			case 3: //Forward + Up axises
				BowDiag();
				break;
		}
	}


}
