using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour , IDamager
{
	public GameObject[] ganchos;
	public GameObject nearGancho;
	public float speed;
	Vector3 dir;
	public GameObject SourceObject => throw new System.NotImplementedException();

	public int GetTeam()
	{
		throw new System.NotImplementedException();
	}
	private void Awake()
	{
		ganchos = GameObject.FindGameObjectsWithTag("Gancho");
		
	}
	public void Gocha(Rigidbody _rigid)
	{
		if(nearGancho != null)
		{
			if (Vector3.Distance(nearGancho.transform.position, transform.position) > 10)
			{
				nearGancho = null;
			}
			else
			{
				dir = nearGancho.transform.position - transform.position;

				_rigid.velocity = new Vector3(dir.x, dir.y, dir.z) * speed;
			}
		}
	}

	public void GetCloseNode()
	{
		//float minDist = Mathf.Infinity;
		float minDist = 10;
		Vector3 currentPos = transform.position;

		foreach (GameObject t in ganchos)
		{
			float dist = Vector3.Distance(t.transform.position, currentPos);
			if (dist < minDist)
			{
				nearGancho = t;
				minDist = dist;
			}
			
		}
	}
	public void WhipAttack(GameObject item, float firstDuration, float secondDuration)
	{
		StartCoroutine(Coroutine_DelayedObjectActiveBlinker(item, firstDuration, secondDuration));
	}
	private IEnumerator Coroutine_DelayedObjectActiveBlinker(GameObject item, float firstDuration, float secondDuration)
	{
		yield return new WaitForSeconds(firstDuration);
		item.SetActive(true);
		yield return new WaitForSeconds(secondDuration);
		item.SetActive(false);
	}
}
