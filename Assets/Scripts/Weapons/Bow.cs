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
		pool = new ObjectPool<Arrow>(ArrownFactory, Arrow.TurnOn, Arrow.TurnOff, 5, false);
	}

	public Arrow ArrownFactory() => Instantiate(arrownPrefab);

	public void ReturnArrow(Arrow item) => pool.ReturnObject(item);

	private void Shoot(TargetDirection direction) {
		var arrow = pool.GetObject();
		if(!arrow)
			return;
		arrow.SetShooter(this);
		var trans = arrow.transform;

		int index = (int)direction;
		trans.position = arrowSources[index].position;
		trans.forward = arrowSources[index].forward;
	}

	public override void Attack(Vector2 direction) {
		bool vertical = direction.y > 0;
		bool horizontal = direction.x != 0;
		TargetDirection directionIndex;

		if(vertical)
			if(horizontal)
				directionIndex = TargetDirection.Diagonal;
			else
				directionIndex = TargetDirection.Vertical;
		else
			directionIndex = TargetDirection.Horizontal; 

		Shoot(directionIndex);

	}

}
