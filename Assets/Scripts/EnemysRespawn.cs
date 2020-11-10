using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemysRespawn : MonoBehaviour
{
    public bool isInitialZone;
    public Vector3 sizeArea;
    public LayerMask enemyLayer;
    private List<Enemy> enemy = new List<Enemy>();
    public LayerMask playerLayer;
    private List<DetectorZone> _colliders = new List<DetectorZone>();
    private DetectorZone inArea;
    private Vector3 finalPos;
    private void Start()
    {
        finalPos = transform.position - new Vector3(0, -sizeArea.y / 2, -sizeArea.z / 2);
        var col = GetComponentsInChildren<DetectorZone>();
        foreach (var item in col)
            _colliders.Add(item);
        DetectEnemyes();
        if(!isInitialZone)
            TurnOffEnemys();
    }

    private void DetectEnemyes()
    {
        var hits = Physics.OverlapBox(finalPos, sizeArea/2, Quaternion.identity, enemyLayer);
        foreach (var item in hits)
        {
            var en = item.GetComponent<Enemy>();
            if (en)
                if (!enemy.Contains(en))
                    enemy.Add(en);
        }
    }

    public void ResetEnemy()
    {
        foreach (var en in enemy)
            en.Reset();
    }

    public void TurnOnEnemys()
    {
        foreach (var en in enemy)
            Enemy.TurnOn(en);
        ResetEnemy();
    }
    
    public void TurnOffEnemys()
    {
        foreach (var en in enemy)
            Enemy.TurnOff(en);
    }
    
    public void DetectPlayer(DetectorZone zone)
    {
        inArea = zone;
        var hits = Physics.OverlapBox(finalPos, sizeArea / 2, Quaternion.identity, playerLayer);
        if(hits.Length == 0)
            EnterZone();
        else
            ExitZone();
    }
    
    private void EnterZone()
    {
        TurnOnEnemys();
        StartCoroutine(DesactivateCollider(inArea));
    }

    private void ExitZone()
    {
        TurnOffEnemys();
        StartCoroutine(DesactivateCollider(inArea));
    }

    IEnumerator DesactivateCollider(DetectorZone zone)
    {
        ModifyCollider(zone, false);
        yield return new WaitForSeconds(2f);
        ModifyCollider(zone, true);
    }

    public void ModifyCollider(DetectorZone zone, bool value) => zone.SetCollider.enabled = value;

    private void OnDrawGizmos()
    {
        var finpos = transform.position - new Vector3(0, -sizeArea.y / 2, -sizeArea.z / 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(finpos, sizeArea);
        Gizmos.color = Color.yellow;
        foreach (var item in enemy)
        {
            if(item)
                Gizmos.DrawLine(transform.position, item.transform.position);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemysRespawn))]
public class EnemysRespawnEditor : Editor
{
    EnemysRespawn _enemysRespawn;

    public override void OnInspectorGUI()
    {
        _enemysRespawn = (EnemysRespawn) target;

        DrawDefaultInspector();
        
        var newStyle = new GUIStyle();
        newStyle.alignment = TextAnchor.MiddleCenter;
        newStyle.normal.textColor = Color.yellow;
        
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Dejar mitad del collider dentro del area y mitad del collider fuera", newStyle );
        EditorGUILayout.Space();
        GUILayout.Label("para que funcione bien el detector.",newStyle);
        if (GUILayout.Button("Crear Detector"))
        {
            var obj = new GameObject("Detector");
            obj.transform.parent = _enemysRespawn.transform;
            obj.AddComponent<DetectorZone>();
            obj.AddComponent<BoxCollider>();
            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.transform.localPosition = Vector3.zero;
            Selection.activeObject = obj;
        }
        EditorGUILayout.EndVertical();

    }
}
#endif