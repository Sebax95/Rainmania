using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform _otherDoor;
    Player player;
    Camera _camera;
    bool _canUse;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
       _camera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if(_canUse)
            if(Input.GetKeyDown(KeyCode.E))
            {
                player.transform.position = _otherDoor.position;
                _camera.transform.position = new Vector3(_otherDoor.transform.position.x, _otherDoor.transform.position.y, _otherDoor.transform.position.z - 6);
            }
    }

   
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
