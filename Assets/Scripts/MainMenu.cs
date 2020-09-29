using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject controls;
    public GameObject credits;
    public GameObject levels;
    bool _true = false;
    bool _true2 = false;
    bool _true3 = false;
    void Start()
    {
        controls.SetActive(false);
        credits.SetActive(false);
        levels.SetActive(false);
    }


    /*private void OnMouseDown()
    {
        Play();
        Controls();
        Credits();
        Exit();
    }*/

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Level2()
    {
        SceneManager.LoadScene(2);
    }

    public void Level3()
    {
        SceneManager.LoadScene(3);
    }

<<<<<<< HEAD
=======
    public void Level4() => SceneManager.LoadScene(4);

>>>>>>> master
    public void Controls()
    {
        _true =! _true;
        _true3 = false;
        controls.SetActive(_true);
        levels.SetActive(false);
    }

    public void Credits()
    {
        _true2 =! _true2;
        _true3 = false;
        credits.SetActive(_true2);
        levels.SetActive(false);
    }

    public void Levels()
    {
        _true = false;
        _true2 = false;
        _true3 = !_true3;
        controls.SetActive(false);
        credits.SetActive(false);
        levels.SetActive(_true3);

    }

    public void Exit()
    {
        Application.Quit();
    }
}
