using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform _otherDoor;
    Player player;
    Camera _camera;
    bool _canUse;
    public int _lock;
    int _key = 0;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
       _camera = FindObjectOfType<Camera>();
       // EventsManager.SubscribeToEvent(EventType.GP_KeyTipe,KeyType);
    }

    void Update()
    {
        if(_canUse)
            if(Input.GetKeyDown(KeyCode.E) && _key <= _lock)
            {
                player.transform.position = _otherDoor.position;
                _camera.transform.position = new Vector3(_otherDoor.transform.position.x, _otherDoor.transform.position.y, _otherDoor.transform.position.z - 6);
            }
    }

   //void KeyType(params object[] parameters)
   //{
   //     _key = (int)parameters[0];
   //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _canUse = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _canUse = false;
    }

}
