using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Transform player;
    public GameObject toSpawn;
    public Transform spawnPos1;
    public Transform spawnPos2;
    public float spawnRate;
    public int spawnLimit;
    public float radiusDetection;

    private float _dist;
    private bool _isEnabled;
    private List<GameObject> spawned = new List<GameObject>();

    private void Start() => StartCoroutine(DetectPlayer());

    void Enable()
    {
        _isEnabled = true;
        StartCoroutine(Spawn());
    }

    IEnumerator DetectPlayer()
    {
        var waiter = new WaitForSeconds(1f);
        while (true)
        {
            yield return waiter;
            _dist = (player.position - transform.position).sqrMagnitude;
            if (_dist < radiusDetection * radiusDetection)
            {
                if (!_isEnabled)
                    Enable();
            }
            else
                if (_isEnabled)
                    Disable();
        }
    }
    
    void Disable() =>_isEnabled = false;

    IEnumerator Spawn()
    {
        var waiter = new WaitForSeconds(spawnRate);
        while (_isEnabled)
        {
            yield return waiter;
            if (spawned.Count < spawnLimit)
            {
                Vector3 newPos = new Vector3(spawnPos1.position.x, Random.Range(spawnPos1.position.y,spawnPos2.position.y), spawnPos1.position.z);
                var obj = Instantiate(toSpawn, newPos, Quaternion.identity);
                spawned.Add(obj);
                obj.GetComponent<Enemy>().spawner = this;
            }

        }
    }
    
    public void DestroyObject(GameObject obj)
    {
        if (spawned.Contains(obj))
        {
            spawned.Remove(obj);
            Destroy(obj);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(spawnPos1.position, new Vector3(.2f,.2f,.2f));
        Gizmos.DrawCube(spawnPos2.position, new Vector3(.2f,.2f,.2f));
        Gizmos.DrawLine(spawnPos1.position, spawnPos2.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusDetection);
    }
}
