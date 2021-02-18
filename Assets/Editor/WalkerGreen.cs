using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;
using UnityEditorInternal;

[CustomEditor(typeof(Embestidor))]
public class WalkerGreen : Editor
{
    Embestidor _redWalker;
    private GUIStyle _labelStyle;
    private GUIStyle _labelCenter;
    private bool _showFoldout;
    private bool _showFoldout2;

    private void OnEnable()
    {
        _redWalker = (Embestidor)target;

        _labelStyle = new GUIStyle();
        _labelStyle.fontStyle = FontStyle.Bold;
        _labelStyle.alignment = TextAnchor.MiddleLeft;
        _labelStyle.fontSize = 20;

        _labelCenter = new GUIStyle();
        _labelCenter.alignment = TextAnchor.MiddleCenter;

        _redWalker.AxisY = true;
        _redWalker.AxisX = true;

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
            Team team = _redWalker.team;
            _redWalker.team = (Team)EditorGUILayout.ObjectField(team, typeof(Team), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _redWalker.maxHealth = EditorGUILayout.FloatField("MaxLife", _redWalker.maxHealth);
            _redWalker.invincibleTime = EditorGUILayout.FloatField("Inmortal Time", _redWalker.invincibleTime);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _redWalker.speed = EditorGUILayout.FloatField("Speed", _redWalker.speed);
            _redWalker.jumpForce = EditorGUILayout.FloatField("Jump Force", _redWalker.jumpForce);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ground Layers");
            LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_redWalker.groundMask), InternalEditorUtility.layers);
            _redWalker.groundMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Front Checker");
            LayerMask tempMask3 = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_redWalker.frontCheckerLayer), InternalEditorUtility.layers);
            _redWalker.frontCheckerLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask3);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Gorund Checker");
            Transform _output = _redWalker.groundChecker;
            _redWalker.groundChecker = (Transform)EditorGUILayout.ObjectField(_output, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();

            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.blue);
            EditorGUILayout.Space();
        }
    }

    void Shoot()
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Attack Options", _labelStyle);
        _showFoldout2 = EditorGUILayout.Foldout(_showFoldout2, "");
        EditorGUILayout.EndHorizontal();

        if (_showFoldout2)
        {
            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.red);

            _redWalker.cdTimer = EditorGUILayout.FloatField("Attack Cooldown", _redWalker.cdTimer);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Head Collider");
            BoxCollider _outputView2 = _redWalker.headCollider;
            _redWalker.headCollider = (BoxCollider)EditorGUILayout.ObjectField(_outputView2, typeof(BoxCollider), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Line of Sight");
            Transform _outputView = _redWalker.offsetLOS;
            _redWalker.offsetLOS = (Transform)EditorGUILayout.ObjectField(_outputView, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("View Angle", _labelCenter);
            _redWalker.viewAngle = EditorGUILayout.Slider(_redWalker.viewAngle, 45, 90);

            EditorGUILayout.LabelField("View Distance", _labelCenter);
            _redWalker.viewDistance = EditorGUILayout.Slider(_redWalker.viewDistance, 5, 70);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Gizmos", _labelCenter);
            GUILayout.BeginHorizontal();
            GUILayout.Label("X");
            _redWalker.AxisX = EditorGUILayout.Toggle(_redWalker.AxisX);
            GUILayout.Label("Y");
            _redWalker.AxisY = EditorGUILayout.Toggle(_redWalker.AxisY);
            GUILayout.EndHorizontal();


            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.red);
        }

    }

    void Gizmos()
    {
        if (_redWalker.AxisX)
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidArc(_redWalker.transform.position, _redWalker.transform.up, _redWalker.transform.forward, _redWalker.viewAngle, _redWalker.viewDistance);
            Handles.DrawSolidArc(_redWalker.transform.position, _redWalker.transform.up, _redWalker.transform.forward, -_redWalker.viewAngle, _redWalker.viewDistance);
            Handles.color = Color.yellow;
            Handles.DrawWireArc(_redWalker.transform.position, _redWalker.transform.up, _redWalker.transform.forward, -_redWalker.viewAngle, _redWalker.viewDistance);
            Handles.DrawWireArc(_redWalker.transform.position, _redWalker.transform.up, _redWalker.transform.forward, _redWalker.viewAngle, _redWalker.viewDistance);
        }
        if (_redWalker.AxisY)
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidArc(_redWalker.transform.position, _redWalker.transform.right, _redWalker.transform.forward, _redWalker.viewAngle, _redWalker.viewDistance);
            Handles.DrawSolidArc(_redWalker.transform.position, _redWalker.transform.right, _redWalker.transform.forward, -_redWalker.viewAngle, _redWalker.viewDistance);
            Handles.color = Color.yellow;
            Handles.DrawWireArc(_redWalker.transform.position, _redWalker.transform.right, _redWalker.transform.forward, -_redWalker.viewAngle, _redWalker.viewDistance);
            Handles.DrawWireArc(_redWalker.transform.position, _redWalker.transform.right, _redWalker.transform.forward, _redWalker.viewAngle, _redWalker.viewDistance);
        }
    }
}
