using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	
	public static UIManager Instance { get { return _Instance;} }
	private static UIManager _Instance;

	private Player _player;
	private Image _healthbar;
	private Text _amountArrows;
	public int _arrows;


	private void Awake() 
	{
		_Instance = this;
		if (!_Instance) Destroy(gameObject);

		_healthbar = transform.Find("Lifebar").GetComponent<Image>();
		_amountArrows = transform.Find("Amount").GetComponent<Text>();
	}

	void Start() 
	{
		_player = FindObjectOfType<Player>();
	}

    private void Update()
    {
		_amountArrows.text = "" + _arrows;
    }

    public void SetHealthbarPercent(float value) 
	{
		_healthbar.fillAmount = value;
	}


}
