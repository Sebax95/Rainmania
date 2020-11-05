using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    public int key;

    private void OnTriggerEnter(Collider other)
    {
    //    if (other.CompareTag("Player"))
    //        EventsManager.TriggerEvent(EventType.GP_KeyTipe, key);
    }
}
