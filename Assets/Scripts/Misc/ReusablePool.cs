using System;
using UnityEngine;
using UnityEngine.Scripting;

using Object = UnityEngine.Object;


public class ReusablePool<T> where T : Component {
	private readonly T _item;
	private readonly Pool<T> _pool;
	private readonly float _delayToDestroy;

	public ReusablePool(T prefab, int initialStock, PoolObject<T>.PoolCallback onEnable, PoolObject<T>.PoolCallback onDisable, bool isDynamic = true, float destroyDelay = 0) {
		_pool = new Pool<T>(initialStock, Factory, onEnable, onDisable, isDynamic);
		_item = prefab;
		_delayToDestroy = destroyDelay;
	}

	public ReusablePool(T prefab, int initialStock, bool isDynamic = true, float destroyDelay = 0) :
		this(prefab, initialStock, OnEnableCallback<T>, OnDisableCallback<T>, isDynamic, destroyDelay) { }

	public ReusablePool(T prefab, float destroyDelay = 0) :
		this(prefab, 0, true, destroyDelay) { }


	private T Factory() => Object.Instantiate(_item);

#pragma warning disable CS0693
	private static void OnEnableCallback<T>(T item) where T : Component => item.gameObject.SetActive(true);
	private static void OnDisableCallback<T>(T item) where T : Component => item.gameObject.SetActive(false);
#pragma warning restore CS0693

	public T GetObject() => _pool.GetObject();
	public void DisableObject(T item) => _pool.DisableObject(item);

	public void Clear() {
		_pool.Clear(x=> Object.Destroy(x,_delayToDestroy));
	}

	~ReusablePool() {
		Clear();
		Debug.LogError($"Te olvidaste de limpiar el ReusablePool de tipo{typeof(T)}. En teoria ya se hizo automaticamente, pero incluí el metodo Clear en el OnDestroy que usa este poo");
	}
}
