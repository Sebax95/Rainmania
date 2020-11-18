using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager Instance { get { return _Instance; } }
	private static UIManager _Instance;

	private Player _player;
	private Image _healthbar;
	Image _Bow;
	Image _Wip;
	private Text _amountArrows;
	public int _arrows;


	private void Awake()
	{
		_Instance = this;
		if (!_Instance) Destroy(gameObject);

		_healthbar = transform.Find("Lifebar").GetComponent<Image>();
		_amountArrows = transform.Find("Amount").GetComponent<Text>();
		_Bow = transform.Find("Arrows").GetComponent<Image>();
		_Wip = transform.Find("Whip").GetComponent<Image>();
	}

	void Start()
	{
		_player = FindObjectOfType<Player>();
		_Bow.enabled = false;
		_Wip.enabled = false;
		_amountArrows.enabled = false;
	}



	public void ArrowAmount(int _arrowss)
	{
		_arrows += _arrowss;
		_amountArrows.text = "" + _arrows;
	}

	public void FoundArcher()
	{
		_Bow.enabled = true;
		_amountArrows.enabled = true;
	}

	public void FoundWhip()
	{
		_Wip.enabled = true;
	}


	public void SetHealthbarPercent(float value)
    {
		_healthbar.fillAmount = value;
	}


}
