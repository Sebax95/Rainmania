using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : TimedBehaviour
{
    public int key;
    public UnityEngine.Events.UnityEvent otherEvents;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventsManager.TriggerEvent(EventsAgus.GP_KeyTipe, key);
            if(otherEvents != null)
                otherEvents.Invoke();
            Destroy(this.gameObject);
        }
    }
}
