using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
	public Arrow arrownPrefab;
	private ObjectPool<Arrow> pool;
	public static Bow Instance
	{
		get { return _Instance; }
	}
	private static Bow _Instance;

	private void Start()
	{
		_Instance = this;
		pool = new ObjectPool<Arrow>(ArrownFactory,Arrow.TurnOn,Arrow.TurnOff, 5 , true);
	}

	public Arrow ArrownFactory()
	{
		return Instantiate(arrownPrefab);
	}

	public void ReturnArrow(Arrow a)
	{
		pool.ReturnObject(a);
	}

	public void BowNormal(Transform PJ/*, GameObject arrow*/)
	{
		//var _arrow = Instantiate(arrow);
		var _arrow = pool.GetObject();
		_arrow.transform.position = PJ.position + PJ.up + PJ.forward;
		_arrow.transform.forward = PJ.forward;
	}
	public void BowUp(Transform PJ/*, GameObject arrow*/)
	{
		//var _arrow = Instantiate(arrow);
		var _arrow = pool.GetObject();

		_arrow.transform.position = PJ.position + new Vector3(0, 2.25f, 0);
		_arrow.transform.forward = PJ.up;
	}
	public void BowDiag(Transform PJ/*, GameObject arrow*/)
	{
		//var _arrow = Instantiate(arrow);
		var _arrow = pool.GetObject();

		_arrow.transform.position = PJ.position + (PJ.up * 2) + (PJ.forward);
		_arrow.transform.forward = PJ.forward + PJ.up;
	}

}
