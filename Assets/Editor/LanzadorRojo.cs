using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;
using UnityEditorInternal;

[CustomEditor(typeof(RedMushroom))]
public class LanzadorRojo : Editor
{
    RedMushroom _redMushroom;
    private GUIStyle _labelStyle;
    private GUIStyle _labelCenter;
    private bool _showFoldout;
    private bool _showFoldout2;

    

    private void OnEnable()
    {
        _redMushroom = (RedMushroom)target;

        _labelStyle = new GUIStyle();
        _labelStyle.fontStyle = FontStyle.Bold;
        _labelStyle.alignment = TextAnchor.MiddleLeft;
        _labelStyle.fontSize = 20;

        _labelCenter = new GUIStyle();
        _labelCenter.alignment = TextAnchor.MiddleCenter;

        _redMushroom.AxisY = true;
        _redMushroom.AxisX = true;

    }

    public override void OnInspectorGUI()
    {
        BasicStats();
        Shoot();

    }

    private void OnSceneGUI()
    {
        Gizmos();
    }

    void BasicStats()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Basic Stats", _labelStyle);
        _showFoldout = EditorGUILayout.Foldout(_showFoldout,"");
        EditorGUILayout.EndHorizontal();

        if (_showFoldout)
        {
            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.blue);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Character Team");
            Team team = _redMushroom.team;
            _redMushroom.team = (Team)EditorGUILayout.ObjectField(team,typeof(Team),true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _redMushroom.maxHealth = EditorGUILayout.FloatField("MaxLife", _redMushroom.maxHealth);
            _redMushroom.invincibleTime = EditorGUILayout.FloatField("Inmortal Time", _redMushroom.invincibleTime);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _redMushroom.speed = EditorGUILayout.FloatField("Speed", _redMushroom.speed);
            _redMushroom.forceJump = EditorGUILayout.FloatField("Trampolin", _redMushroom.forceJump);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ground Layers");
            LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_redMushroom.groundMask), InternalEditorUtility.layers);
            _redMushroom.groundMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            EditorGUILayout.EndHorizontal();

            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.blue);
            EditorGUILayout.Space();
        }
    }

    void Shoot()
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Shoot Options", _labelStyle);
        _showFoldout2 = EditorGUILayout.Foldout(_showFoldout2, "");
        EditorGUILayout.EndHorizontal();

        if (_showFoldout2)
        {
            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.red);

            EditorGUILayout.LabelField("Bullet Prefab", _labelCenter);
            PoisonBullet bullet = _redMushroom.bulletPref;
            _redMushroom.bulletPref = (PoisonBullet)EditorGUILayout.ObjectField(bullet, typeof(PoisonBullet), true);

            _redMushroom.cdTimer = EditorGUILayout.FloatField("Fire Cooldown", _redMushroom.cdTimer);
            Transform _output = _redMushroom.output;
            _redMushroom.output = (Transform)EditorGUILayout.ObjectField(_output,typeof(Transform),true);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Line of Sight");
            Transform _outputView = _redMushroom.offsetLOS;
            _redMushroom.offsetLOS = (Transform)EditorGUILayout.ObjectField(_outputView, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("View Angle", _labelCenter);
            _redMushroom.viewAngle = EditorGUILayout.Slider(_redMushroom.viewAngle,45,90);
          
            EditorGUILayout.LabelField("View Distance", _labelCenter);
            _redMushroom.viewDistance = EditorGUILayout.Slider(_redMushroom.viewDistance, 5,70);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Gizmos", _labelCenter);
            GUILayout.BeginHorizontal();
            GUILayout.Label("X");
            _redMushroom.AxisX = EditorGUILayout.Toggle(_redMushroom.AxisX);
            GUILayout.Label("Y");
            _redMushroom.AxisY = EditorGUILayout.Toggle(_redMushroom.AxisY);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.red);
        }

    }

    void Gizmos()
    {
        if (_redMushroom.AxisX)
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidArc(_redMushroom.transform.position, _redMushroom.transform.up, _redMushroom.transform.forward, _redMushroom.viewAngle, _redMushroom.viewDistance);
            Handles.DrawSolidArc(_redMushroom.transform.position, _redMushroom.transform.up, _redMushroom.transform.forward, -_redMushroom.viewAngle, _redMushroom.viewDistance);
            Handles.color = Color.yellow;
            Handles.DrawWireArc(_redMushroom.transform.position, _redMushroom.transform.up, _redMushroom.transform.forward, -_redMushroom.viewAngle, _redMushroom.viewDistance);
            Handles.DrawWireArc(_redMushroom.transform.position, _redMushroom.transform.up, _redMushroom.transform.forward, _redMushroom.viewAngle, _redMushroom.viewDistance);
        }
        if (_redMushroom.AxisY)
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidArc(_redMushroom.transform.position, _redMushroom.transform.right, _redMushroom.transform.forward, _redMushroom.viewAngle, _redMushroom.viewDistance);
            Handles.DrawSolidArc(_redMushroom.transform.position, _redMushroom.transform.right, _redMushroom.transform.forward, -_redMushroom.viewAngle, _redMushroom.viewDistance);
            Handles.color = Color.yellow;
            Handles.DrawWireArc(_redMushroom.transform.position, _redMushroom.transform.right, _redMushroom.transform.forward, -_redMushroom.viewAngle, _redMushroom.viewDistance);
            Handles.DrawWireArc(_redMushroom.transform.position, _redMushroom.transform.right, _redMushroom.transform.forward, _redMushroom.viewAngle, _redMushroom.viewDistance);
        }
    }
}
