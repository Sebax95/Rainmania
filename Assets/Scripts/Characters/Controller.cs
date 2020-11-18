using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public abstract class Controller : TimedBehaviour {
	public int ID { get; protected set; }
	//protected Controllable basePawn;

	protected abstract void DoMovement();
	protected abstract void DoActions();

	protected override void OnUpdate(){
		DoMovement();
		DoActions();
	}

	//public virtual void AssignUser(Controllable user) {
	//	basePawn = user;
	//}

	protected T User<T>() where T : Controllable => ControllerHandler.Instance.GetUser(this) as T;

	public static T Create<T>() where T : Controller => new GameObject("Controller", typeof(T)).GetComponent<T>();

	public virtual void DestroyController() => Destroy(gameObject);
}
