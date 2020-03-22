using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform baseCamera;
    public Transform player;
    public float smoothMovement = 7;

    public void Start()
    {
        transform.position = baseCamera.position;
    }

    private void FixedUpdate()
    {
        var smooth = Vector3.Slerp(transform.position, baseCamera.position, Time.deltaTime * smoothMovement);
        transform.position = smooth;
        //transform.LookAt(player);
    }
}
