using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;
using UnityEditorInternal;


[CustomEditor(typeof(GreenMushroom))]
public class LanzadorVerde : Editor
{
    GreenMushroom _greenMushroom;
    private GUIStyle _labelStyle;
    private GUIStyle _labelCenter;
    private bool _showFoldout;
    private bool _showFoldout2;



    private void OnEnable()
    {
        _greenMushroom = (GreenMushroom)target;

        _labelStyle = new GUIStyle();
        _labelStyle.fontStyle = FontStyle.Bold;
        _labelStyle.alignment = TextAnchor.MiddleLeft;
        _labelStyle.fontSize = 20;

        _labelCenter = new GUIStyle();
        _labelCenter.alignment = TextAnchor.MiddleCenter;
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
        _showFoldout = EditorGUILayout.Foldout(_showFoldout, "");
        EditorGUILayout.EndHorizontal();

        if (_showFoldout)
        {
            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.blue);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Character Team");
            Team team = _greenMushroom.team;
            _greenMushroom.team = (Team)EditorGUILayout.ObjectField(team, typeof(Team), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _greenMushroom.maxHealth = EditorGUILayout.FloatField("MaxLife", _greenMushroom.maxHealth);
            _greenMushroom.invincibleTime = EditorGUILayout.FloatField("Inmortal Time", _greenMushroom.invincibleTime);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _greenMushroom.speed = EditorGUILayout.FloatField("Speed", _greenMushroom.speed);
            _greenMushroom.forceJump = EditorGUILayout.FloatField("Trampolin", _greenMushroom.forceJump);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ground Layers");
            LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_greenMushroom.groundMask), InternalEditorUtility.layers);
            _greenMushroom.groundMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
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
            PoisonBullet bullet = _greenMushroom.bulletPref;
            _greenMushroom.bulletPref = (PoisonBullet)EditorGUILayout.ObjectField(bullet, typeof(PoisonBullet), true);

            _greenMushroom.cdTimer = EditorGUILayout.FloatField("Fire Cooldown", _greenMushroom.cdTimer);
            Transform _output = _greenMushroom.output;
            _greenMushroom.output = (Transform)EditorGUILayout.ObjectField(_output, typeof(Transform), true);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Line of Sight");
            Transform _outputView = _greenMushroom.offsetLOS;
            _greenMushroom.offsetLOS = (Transform)EditorGUILayout.ObjectField(_outputView, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("View Angle", _labelCenter);
            _greenMushroom.viewAngle = EditorGUILayout.Slider(_greenMushroom.viewAngle, 45, 90);

            EditorGUILayout.LabelField("View Distance", _labelCenter);
            _greenMushroom.viewDistance = EditorGUILayout.Slider(_greenMushroom.viewDistance, 5, 70);

            EditorGUILayout.LabelField("Bullet Hight", _labelCenter);
            _greenMushroom.altBullet = EditorGUILayout.Slider(_greenMushroom.altBullet, -10, 10);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Gizmos", _labelCenter);
            GUILayout.BeginHorizontal();
            GUILayout.Label("X");
            _greenMushroom.AxisX = EditorGUILayout.Toggle(_greenMushroom.AxisX);
            GUILayout.Label("Y");
            _greenMushroom.AxisY = EditorGUILayout.Toggle(_greenMushroom.AxisY);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.red);
        }

    }

    void Gizmos()
    {
        if (_greenMushroom.AxisX)
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidArc(_greenMushroom.transform.position, _greenMushroom.transform.up, _greenMushroom.transform.forward, _greenMushroom.viewAngle, _greenMushroom.viewDistance);
            Handles.DrawSolidArc(_greenMushroom.transform.position, _greenMushroom.transform.up, _greenMushroom.transform.forward, -_greenMushroom.viewAngle, _greenMushroom.viewDistance);
            Handles.color = Color.yellow;
            Handles.DrawWireArc(_greenMushroom.transform.position, _greenMushroom.transform.up, _greenMushroom.transform.forward, -_greenMushroom.viewAngle, _greenMushroom.viewDistance);
            Handles.DrawWireArc(_greenMushroom.transform.position, _greenMushroom.transform.up, _greenMushroom.transform.forward, _greenMushroom.viewAngle, _greenMushroom.viewDistance);
        }
        if (_greenMushroom.AxisY)
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidArc(_greenMushroom.transform.position, _greenMushroom.transform.right, _greenMushroom.transform.forward, _greenMushroom.viewAngle, _greenMushroom.viewDistance);
            Handles.DrawSolidArc(_greenMushroom.transform.position, _greenMushroom.transform.right, _greenMushroom.transform.forward, -_greenMushroom.viewAngle, _greenMushroom.viewDistance);
            Handles.color = Color.yellow;
            Handles.DrawWireArc(_greenMushroom.transform.position, _greenMushroom.transform.right, _greenMushroom.transform.forward, -_greenMushroom.viewAngle, _greenMushroom.viewDistance);
            Handles.DrawWireArc(_greenMushroom.transform.position, _greenMushroom.transform.right, _greenMushroom.transform.forward, _greenMushroom.viewAngle, _greenMushroom.viewDistance);
        }
    }
}
