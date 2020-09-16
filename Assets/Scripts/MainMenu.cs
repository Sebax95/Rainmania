using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject controls;
    public GameObject credits;
    bool _true = false;
    bool _true2 = false;
    void Start()
    {
        controls.SetActive(false);
        credits.SetActive(false);
    }


    private void OnMouseDown()
    {
        Play();
        Controls();
        Credits();
        Exit();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
        _true =! _true;
        controls.SetActive(_true);
    }

    public void Credits()
    {
        _true2 =! _true2;
        credits.SetActive(_true2);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
