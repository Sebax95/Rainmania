using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : TimedBehaviour {

    public static GameManager Instance { get; private set; }
    Action test;
    public bool IsPause => !TimedBehaviour.RunUpdates;

    private void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //no me putees manu, taba probando algo piola
    IEnumerator WaitAndExecute(float timer, Action function)
    {
        yield return new WaitForSeconds(timer);
        function();
    }

    public void PlayerDie()
    {
        test += LoadActualScene;
        StartCoroutine(WaitAndExecute(5.5f, test));
        test -= LoadActualScene;
    }

    public void LoadActualScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void DestroyOnMenu()
    {
        Destroy(gameObject);
    }
}
