using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continuara : MonoBehaviour
{
    float timer;

    // Update is called once per frame
    void Update()
    {
        timer += 1 * Time.deltaTime;
        if (timer >= 2)
        {
            SceneManager.LoadScene(0);
        }
    }
}
