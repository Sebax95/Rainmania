using System;
using System.Collections.Generic;

public class Pool<T> {
	public delegate T CallbackFactory();
    private readonly List<PoolObject<T>> thisPool;
	private CallbackFactory factoryMethod;
	private PoolObject<T>.PoolCallback initializeMethod;
	private PoolObject<T>.PoolCallback terminateMethod;
	private bool isDynamicPool;

    //public List<PoolObject<T>> InternalList => thisPool;

	public Pool(int initialStock, CallbackFactory factory, PoolObject<T>.PoolCallback initialize,
		PoolObject<T>.PoolCallback terminate, bool isDynamic = true) {
		factoryMethod = factory;
		initializeMethod = initialize;
		terminateMethod = terminate;
		isDynamicPool = isDynamic;

		thisPool = new List<PoolObject<T>>();
		for (int i = 0; i < initialStock; i++) { 
			thisPool.Add(new PoolObject<T>(factory(), initialize, terminate));
		}
	}

	public T GetObject() {
		for (int i = thisPool.Count - 1; i >= 0; i--) {
			if (!thisPool[i].IsActive) {
				thisPool[i].IsActive = true;
				return thisPool[i].GetObject;
			}
		}
		if (isDynamicPool) {
			var po = new PoolObject<T>(factoryMethod(), initializeMethod, terminateMethod);
			thisPool.Add(po);
			po.IsActive = true;
			return po.GetObject;
		} else {
			return default(T);
		}
	}

	public void DisableObject(T obj) {
		foreach (var item in thisPool) {
			if (item.GetObject.Equals(obj)) {
				item.IsActive = false;
				return;
			}
		}
	}

	public void Clear() {
		thisPool.ForEach(x => x.Clear());
		thisPool.Clear();
	}

	public void Clear(Action<T> objectDestructor) {
		thisPool.ForEach(x => x.Clear(objectDestructor));
		thisPool.Clear();
	}

	~Pool() {
		factoryMethod = null;
		Clear();
	}
}



public class PoolObject<T>{

	private bool isActive;
    private T thisObject;

	public delegate void PoolCallback(T obj);
	private PoolCallback activateMethod;
	private PoolCallback deactivateMethod;

	public PoolObject(T obj, PoolCallback intialize, PoolCallback terminate){
        thisObject = obj;
		activateMethod = intialize;
		deactivateMethod = terminate;
    }

    public T GetObject { 
		get {
            return thisObject;
		}
	}

	public bool IsActive {
		get {
			return isActive;
		}
		set {
			isActive = value;
			if (isActive) {
				activateMethod(thisObject);
			} else {
				deactivateMethod(thisObject);
			}
		}
	}

	public void Clear() {
		activateMethod = null;
		deactivateMethod = null;
	}

	public void Clear(Action<T> objectDestructor) {
		objectDestructor(thisObject);
		Clear();
	}

	~PoolObject() {
		Clear();
	}
}	