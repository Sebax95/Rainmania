using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public Transform newPosition;
    public Camera camera;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = newPosition.transform.position;
            camera.transform.position = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, newPosition.transform.position.z - 6) ;
        }
    }
}
