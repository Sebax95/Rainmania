using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class FOVEditor : Editor
{
    Enemy fov;

    private void OnSceneGUI()
    {
        fov = (Enemy)target;
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.right, 360, fov.viewDistance);
        Handles.DrawWireArc(fov.transform.position, Vector3.back, Vector3.right, -fov.viewAngle /2 , fov.viewDistance);
        Handles.DrawWireArc(fov.transform.position, Vector3.back, Vector3.right, fov.viewAngle /2 , fov.viewDistance);
        
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewDistance);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewDistance);


        Handles.DrawLine(fov.transform.position, fov.target.transform.position);
    }
    
}
