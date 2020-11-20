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
    public AudioSource audioS;
    bool _true = false;
    bool _true2 = false;
    bool _true3 = false;
    void Start()
    {
        controls.SetActive(false);
        credits.SetActive(false);
        levels.SetActive(false);
        audioS = GetComponent<AudioSource>();
    }


    /*private void OnMouseDown()
    {
        Play();
        Controls();
        Credits();
        Exit();
    }*/

    IEnumerator Corrutine_PLay(int indexlvl) 
    {
        audioS.Play();
        yield return new WaitForSeconds(0.5F);
        SceneManager.LoadScene(indexlvl);

    }

    IEnumerator Corrutine_Exit()
    {
        audioS.Play();
        yield return new WaitForSeconds(0.5F);
        Application.Quit();

    }


    public void Play()
    {
        StartCoroutine(Corrutine_PLay(1));
        
    }

    public void Level2()
    {
        StartCoroutine(Corrutine_PLay(2));
    }

    public void Level3()
    {
        StartCoroutine(Corrutine_PLay(3));

    }

    public void Controls()
    {
        audioS.Play();
        _true =! _true;
        _true3 = false;
        controls.SetActive(_true);
        levels.SetActive(false);
    }

    public void Credits()
    {
        audioS.Play();
        _true2 =! _true2;
        _true3 = false;
        credits.SetActive(_true2);
        levels.SetActive(false);
    }

    public void Levels()
    {
        audioS.Play();
        _true = false;
        _true2 = false;
        _true3 = !_true3;
        controls.SetActive(false);
        credits.SetActive(false);
        levels.SetActive(_true3);

    }

    public void Exit()
    {
        StartCoroutine(Corrutine_Exit());
        
    }
}
