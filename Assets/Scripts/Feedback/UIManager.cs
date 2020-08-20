using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	private Player _player;

	private Image _healthbar;

	private void Awake() {
		_healthbar = transform.Find("Lifebar").GetComponent<Image>();
	}

	// Start is called before the first frame update
	void Start() {
		_player = FindObjectOfType<Player>();
	}

	public void SetHealthbarPercent(float value) {
		_healthbar.fillAmount = value;
	}


}
