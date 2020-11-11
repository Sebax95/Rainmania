using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;

public class Bow : Weapon {

	private int maxArrowcount = 5;
	public Arrow arrownPrefab;
	private Pool<Arrow> pool;
	public Transform[] arrowSources;

	public float attackWindup;
	public float attackCooldown;
	public float wallDistanceCheck = 1;
	public float closeWallArrowFixOffset;
	public float timeToReturnArrow;
	public override float FullAttackDuration => attackWindup + attackCooldown;
	private WaitForSeconds wait_shootWindup;
	private WaitForSeconds wait_arrowReturnTime;

	private IWielder wielder;
	private CrouchStateRouter router;
	public override Team GetTeam => wielder.GetTeam;
	public override GameObject SourceObject => gameObject;

	public const string NAME = "Bow";
	public override string Name => NAME;

	private void Start() {
		// motivo por el cual no ataca al tpque.
		wait_shootWindup = new WaitForSeconds(attackWindup);
		wait_arrowReturnTime = new WaitForSeconds(timeToReturnArrow);
		wielder = GetComponent<IWielder>();
		router = GetComponent<CrouchStateRouter>();
		InitializePool();
		UpdateStateOnUpgrade(UpgradesManager.Instance.Data);
	}

	public Arrow ArrownFactory() {
		var item = Instantiate(arrownPrefab);
		item.gameObject.SetActive(false);
		return item;
	}

	public void ReturnArrow(Arrow item) => pool.DisableObject(item);

	private void Shoot(TargetDirection direction) {
		var arrow = pool.GetObject();
		if(!arrow)
			return;
		arrow.SetShooter(this);
		var trans = arrow.transform;

		int index = (int)direction;
		Transform source = router.Current.arrowSpawns[index];
		trans.position = source.position;
		trans.rotation = source.rotation;

		RaycastHit hit;
		if(Physics.Raycast(transform.position,trans.forward,out hit, wallDistanceCheck, ~MiscUnityUtilities.IntToLayerMask(gameObject.layer)))
		{
			trans.position = hit.point + trans.forward * closeWallArrowFixOffset;
		}
		GameManager.Instance.StartCoroutine(Coroutine_ReturnArrowToPool(arrow));

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

	private void InitializePool() {
		if(pool != null)
			pool.Clear(Destroy);

		pool = new Pool<Arrow>(maxArrowcount, ArrownFactory, Arrow.TurnOn, Arrow.TurnOff, false);
	}

	private void UpdateStateOnUpgrade(UpgradesData data) {
		if(data == null)
			return;
		attackCooldown = data.GetFloat("arrowShootSpeed");
		maxArrowcount = data.GetInt("arrowMaxAmount");
		damage = data.GetInt("arrowDamage");
		arrownPrefab.canPlatform = data.GetBool("arrowCanPlatform");
		arrownPrefab.canAnchor = data.GetBool("arrowCanAnchor");
		InitializePool();
	}

	private IEnumerator Coroutine_ReturnArrowToPool(Arrow item) {
		yield return wait_arrowReturnTime;
		ReturnArrow(item);
	}

	private void OnEnable() => UpgradesManager.Instance.OnUpdateData += UpdateStateOnUpgrade;
	private void OnDisable() => UpgradesManager.Instance.OnUpdateData -= UpdateStateOnUpgrade;
}
