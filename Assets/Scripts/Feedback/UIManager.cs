using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	public GameObject _pauseMenu;
	bool _active;



	private void Awake()
	{
		_Instance = this;
		if (!_Instance) Destroy(gameObject);

		_healthbar = transform.Find("Lifebar").GetComponent<Image>();
		_amountArrows = transform.Find("Amount").GetComponent<Text>();
		_Bow = transform.Find("Arrows").GetComponent<Image>();
		_Wip = transform.Find("Whip").GetComponent<Image>();
		_active = false;
		
	}

	void Start()
	{
		_player = FindObjectOfType<Player>();
		_Bow.enabled = false;
		_Wip.enabled = false;
		_amountArrows.enabled = false;
		_pauseMenu.SetActive(_active);
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
			_active = !_active;
			_pauseMenu.SetActive(_active);
        }
    }

	public void BackMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void Exit()
    {
		Application.Quit();
	}

	public void Resume()
    {
		_active = false;
		_pauseMenu.SetActive(false);
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
