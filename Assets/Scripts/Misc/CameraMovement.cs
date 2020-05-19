using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;
    public LayerMask validFloorLayers;
    public float smoothMovement;
    public float sumDistance, clampDistance;
    public float minDistance;
    public float valorMinimoInicio;

    public void Start()
    {
        transform.position = player.position + offset;
    }

    private void FixedUpdate()
    {
        var smooth = Vector3.Slerp(transform.position, player.position + offset, Time.deltaTime * smoothMovement);
        transform.position = smooth;
        ModifyingOffsetZ();        
    }

    public void ModifyingOffsetZ()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position + Vector3.up * 1, Vector3.down, out hit, Mathf.Infinity, validFloorLayers))
        {
            var distance = hit.distance * sumDistance;
            distance = Mathf.Clamp(distance, minDistance, clampDistance);
            if (hit.distance >= valorMinimoInicio)
                offset.z = Mathf.Lerp(offset.z, distance, Time.deltaTime * 5);
            else
                offset.z = Mathf.Lerp(offset.z, minDistance, Time.deltaTime * 5);
        }
    }
}
