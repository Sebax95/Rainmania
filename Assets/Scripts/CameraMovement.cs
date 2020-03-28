using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;
    public float smoothMovement = 7;

    public void Start()
    {
        transform.position = player.position + offset;
    }

    private void FixedUpdate()
    {
        var smooth = Vector3.Slerp(transform.position, player.position + offset, Time.deltaTime * smoothMovement);
        transform.position = smooth;
        //transform.LookAt(player);
    }
}
