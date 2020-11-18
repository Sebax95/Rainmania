using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : TimedBehaviour
{
    public int key;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventsManager.TriggerEvent(EventsAgus.GP_KeyTipe, key);
            Destroy(this.gameObject);
        }
    }
}
