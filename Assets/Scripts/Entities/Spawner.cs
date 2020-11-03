using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Transform player;
    public Enemy toSpawn;
    public Transform spawnPos1;
    public Transform spawnPos2;
    public float spawnRate;
    public int spawnLimit;
    public float radiusDetection;

    private float _dist;
    private bool _isEnabled;
    [SerializeField]
    private List<Enemy> spawned = new List<Enemy>();

    private ReusablePool<Enemy> _pool;

    private void Awake() => _pool = new ReusablePool<Enemy>(toSpawn, spawnLimit, Enemy.TurnOn, Enemy.TurnOff, false);

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
            if (spawned.Count < spawnLimit)
            {
                Vector3 newPos = new Vector3(spawnPos1.position.x, Random.Range(spawnPos1.position.y,spawnPos2.position.y), spawnPos1.position.z);
                var obj = _pool.GetObject();
                obj.SetValues( newPos, spawnPos1.forward);
                spawned.Add(obj);
                obj.spawner = this;
            }
            yield return waiter;

        }
    }
    
    public void DestroyObject(Enemy obj)
    {
        if (spawned.Contains(obj))
        {
            spawned.Remove(obj);
            _pool.DisableObject(obj);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(spawnPos1.position, spawnPos1.position + spawnPos1.forward * 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(spawnPos1.position, new Vector3(.2f,.2f,.2f));
        Gizmos.DrawCube(spawnPos2.position, new Vector3(.2f,.2f,.2f));
        Gizmos.DrawLine(spawnPos1.position, spawnPos2.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusDetection);
    }
}
