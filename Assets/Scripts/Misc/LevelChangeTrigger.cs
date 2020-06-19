using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeTrigger : MonoBehaviour {
	public int levelIndex;
	public Animator anim;

	private void OnTriggerEnter(Collider other) {
		if(!other.gameObject.GetComponent<Player>())
			return;
		
		FadeToLevel(levelIndex);
		//SceneManager.LoadScene(levelIndex);
	}

#if UNITY_EDITOR
	private BoxCollider collidr;
	private void OnDrawGizmos() {
		if(!collidr)
			collidr = GetComponent<BoxCollider>();
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position + collidr.center, collidr.size);
	}
#endif

	public void FadeToLevel(int levelIndex) 
	{
		anim.SetTrigger("FadeOut");
	}

	public void OnFadeComplete() 
	{
		SceneManager.LoadScene(levelIndex);
	}
}
