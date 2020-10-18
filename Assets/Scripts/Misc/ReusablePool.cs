using System;
using UnityEngine;

using Object = UnityEngine.Object;

public class ReusablePool<T> where T : MonoBehaviour, IDisposable {
	private readonly T _item;
	private readonly Pool<T> _pool;
	private readonly float _delayToDestroy;

	public ReusablePool(T prefab, int initialStock, PoolObject<T>.PoolCallback onEnable, PoolObject<T>.PoolCallback onDisable, bool isDynamic = true, float destroyDelay = 0) {
		_pool = new Pool<T>(initialStock, Factory, onEnable, onDisable, isDynamic);
		_item = prefab;
		_delayToDestroy = destroyDelay;
	}

	public ReusablePool(T prefab, int initialStock, bool isDynamic = true, float destroyDelay = 0) {
		_pool = new Pool<T>(initialStock, Factory, OnEnableCallback, OnDisableCallback, isDynamic);
		_item = prefab;
		_delayToDestroy = destroyDelay;
	}

	public ReusablePool(T prefab, float destroyDelay = 0) {
		_pool = new Pool<T>(0, Factory, OnEnableCallback, OnDisableCallback, true);
		_item = prefab;
		_delayToDestroy = destroyDelay;
	}

	private T Factory() => Object.Instantiate(_item);
	private void OnEnableCallback(T item) => item.gameObject.SetActive(true);
	private void OnDisableCallback(T item) => item.gameObject.SetActive(false);

	public T GetObject() => _pool.GetObject();
	public void DisableObject(T item) => _pool.DisableObject(item);

	public void Clear() =>
		_pool.InternalList.ForEach(x => Object.Destroy(x.GetObject,_delayToDestroy));

	public void Dispose() => Clear();
}
