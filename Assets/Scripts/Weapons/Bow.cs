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
	public float arrowRegenSpeed;
	public float arrowDespawnTime;
	public override float FullAttackDuration => attackWindup + attackCooldown;
	private WaitForSeconds wait_shootWindup;
	private WaitForSeconds wait_arrowReturnTime;
	private CooldownTracker cdTracker;

	private IWielder wielder;
	private CrouchStateRouter router;
	public override Team GetTeam => wielder.GetTeam;
	public override GameObject SourceObject => gameObject;

	public const string NAME = "Bow";
	public override string Name => NAME;

	private void Start() {
		// motivo por el cual no ataca al tpque.
		wait_shootWindup = new WaitForSeconds(attackWindup);
		wait_arrowReturnTime = new WaitForSeconds(arrowDespawnTime);
		cdTracker = new CooldownTracker(arrowRegenSpeed, maxArrowcount);
		wielder = GetComponent<IWielder>();
		router = GetComponent<CrouchStateRouter>();
		//InitializePool();
		cdTracker.OnAmountUpdate += UIManager.Instance.SetArrowAmount;
		UpdateStateOnUpgrade(UpgradesManager.Instance.Data);
	}

	protected override void OnUpdate() {
		base.OnUpdate();
		cdTracker.Update(Time.deltaTime);
	}

	public Arrow ArrownFactory() {
		var item = Instantiate(arrownPrefab);
		item.gameObject.SetActive(false);
		return item;
	}

	public void ReturnArrow(Arrow item) => pool.DisableObject(item);

	private void Shoot(TargetDirection direction) {
		if(!cdTracker.TryTriggerOnce())
			return;
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
		if(Physics.Raycast(transform.position,trans.forward,out hit, wallDistanceCheck, ~MiscUnityUtilities.IntToLayerMask(gameObject.layer),QueryTriggerInteraction.Ignore))
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
			pool.Clear(DestroyGO);

		pool = new Pool<Arrow>(maxArrowcount, ArrownFactory, Arrow.TurnOn, Arrow.TurnOff, true);
	}

	private void UpdateStateOnUpgrade(GenericDataPack data) {
		if(data == null)
			return;
		attackCooldown = data.GetFloat("arrowShootSpeed");
		maxArrowcount = data.GetInt("arrowMaxAmount");
		UIManager.Instance._arrows = data.GetInt("arrowMaxAmount");
		damage = data.GetInt("arrowDamage");
		arrownPrefab.canPlatform = data.GetBool("arrowCanPlatform");
		arrownPrefab.canAnchor = data.GetBool("arrowCanAnchor");
		cdTracker.SetMaxCount(maxArrowcount);
		cdTracker.SetCooldown(arrowRegenSpeed);
		cdTracker.ForceRefresh();
		InitializePool();
	}

	private IEnumerator Coroutine_ReturnArrowToPool(Arrow item) {
		yield return wait_arrowReturnTime;
		if(!this)
			yield break;
		ReturnArrow(item);
	}

	private void DestroyGO(Arrow item) => Destroy(item.gameObject);

	private void OnEnable() => UpgradesManager.Instance.OnUpdateData += UpdateStateOnUpgrade;
	private void OnDisable() => UpgradesManager.Instance.OnUpdateData -= UpdateStateOnUpgrade;
}

public class CooldownTracker {
	private int _maxCount;
	private float _cdDuration;
	private float _cdTimer;

	public int AvailableCount { get;  private set; }

	public bool CanTrigger => AvailableCount > 0;
	public float CurrentCdPercent => _cdTimer / _cdDuration;

	public event System.Action<int> OnAmountUpdate;

	public CooldownTracker(float cooldownDuration, int maxAmount = 1) {
		_cdDuration = cooldownDuration;
		_maxCount = maxAmount;
		ForceRefresh();
	}

	public bool TryTriggerOnce() {
		if(!CanTrigger)
			return false;

		AvailableCount--;
		OnAmountUpdate?.Invoke(AvailableCount);

		return true;
	}

	public int TryTriggerMultiple(int amount) {
		int total = Mathf.Min(amount, AvailableCount);
		AvailableCount -= total;
		OnAmountUpdate?.Invoke(AvailableCount);
		return total;
	}

	public void Update(float deltaTime) {
		if(AvailableCount >= _maxCount)
		{
			_cdTimer = 0;
			return;
		}
		_cdTimer += deltaTime;
		if(_cdTimer < _cdDuration)
			return;
		AvailableCount += 1;
		_cdTimer -= _cdDuration;

		OnAmountUpdate?.Invoke(AvailableCount);
	}

	public void ForceRefresh() {
		AvailableCount = _maxCount;
		_cdTimer = 0;

		OnAmountUpdate?.Invoke(AvailableCount);
	}


	public void SetCooldown(float duration) => _cdDuration = duration;
	public void SetMaxCount(int amount) => _maxCount = amount;
}