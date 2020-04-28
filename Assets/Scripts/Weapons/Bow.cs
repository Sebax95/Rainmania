﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {

	private int INITAL_ARROW_COUNT = 5;
	public Arrow arrownPrefab;
	private Pool<Arrow> pool;
	public Transform[] arrowSources;

	public float attackWindup;
	public float attackCooldown;
	public override float FullAttackDuration => attackWindup + attackCooldown;
	private WaitForSeconds wait_shootWindup;

	private IWielder wielder;
	public override Team GetTeam => wielder.GetTeam;
	public override GameObject SourceObject => gameObject;

	public const string NAME = "Bow";
	public override string Name => NAME;

	private void Start() {
		wait_shootWindup = new WaitForSeconds(attackWindup);
		wielder = GetComponent<IWielder>();
		pool = new Pool<Arrow>(INITAL_ARROW_COUNT ,ArrownFactory, Arrow.TurnOn, Arrow.TurnOff, false);
	}

	private void Update() {
		
	}

	public Arrow ArrownFactory() => Instantiate(arrownPrefab);

	public void ReturnArrow(Arrow item) => pool.DisableObject(item);

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

		//Shoot(directionIndex);
		StartCoroutine(Coroutine_DelayedShoot(directionIndex));
	}

	private IEnumerator Coroutine_DelayedShoot(TargetDirection direction) {
		yield return wait_shootWindup;
		Shoot(direction);
	}
}
