using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TrailWhipPool : MonoBehaviour
{
    private TrailWhipEffect sourcePool;

    private void Awake()=> TurnOff(this);

    private void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        StartCoroutine(WaitForDie());
    }

    IEnumerator WaitForDie()
    {
        yield return new WaitForSeconds(1f);
        sourcePool.trailPool.DisableObject(this);
    }

    public static void TurnOff(TrailWhipPool item) => item.gameObject.SetActive(false);
    public static void TurnOn(TrailWhipPool item)
    {
        item.gameObject.SetActive(true);
        item.Reset();
    }

    public void SetSource(TrailWhipEffect source) => sourcePool = source;

    public void SetValues(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
    
}
