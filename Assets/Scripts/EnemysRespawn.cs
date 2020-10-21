using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysRespawn : MonoBehaviour
{
    public Enemy[] enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                //enemy[i].Reset;
            }
        }
    }
}
