using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysRespawn : MonoBehaviour
{
    public Vector3 sizeArea;
    public LayerMask enemyLayer;
    [SerializeField]
    public List<Enemy> enemy = new List<Enemy>();

    private Vector3 finalPos;
    private void Start()
    {
        finalPos = transform.position - new Vector3(0, -sizeArea.y / 2, -sizeArea.z / 2);
        DetectEnemyes();
    }


    public void DetectEnemyes()
    {
        var half = Vector3.one;
        var direction = new Vector3(sizeArea.x - transform.position.x, sizeArea.y - transform.position.y,
            sizeArea.z - transform.position.z);
        var hits = Physics.BoxCastAll(finalPos, sizeArea/2, new Vector3(1, 0, 1), Quaternion.identity, enemyLayer);
        foreach (var item in hits)
        {
            var en = item.collider.GetComponent<Enemy>();
            if (en)
                if (!enemy.Contains(en))
                    enemy.Add(en);
        }
    }



    private void OnDrawGizmos()
    {
        var finpos = transform.position - new Vector3(0, -sizeArea.y / 2, -sizeArea.z / 2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(finpos, sizeArea/2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(finpos, sizeArea);
        Gizmos.color = Color.yellow;
        foreach (var item in enemy)
        {
            if(item)
                Gizmos.DrawLine(transform.position, item.transform.position);
        }
    }
}
