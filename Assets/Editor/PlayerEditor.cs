using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 
using UnityEditor.AnimatedValues; 
using System;
using UnityEditorInternal;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    Player _player;
    private GUIStyle _labelStyle;
    private GUIStyle _MiniCenter;
    private bool _showFoldout;


    private void OnEnable()
    {
        _player = (Player)target;

        _labelStyle = new GUIStyle();
        _labelStyle.fontStyle = FontStyle.Bold;
        _labelStyle.alignment = TextAnchor.MiddleCenter;
        _labelStyle.fontSize = 20;

        _MiniCenter = new GUIStyle();
        _MiniCenter.fontStyle = FontStyle.Bold;
        _MiniCenter.alignment = TextAnchor.MiddleCenter;
        _MiniCenter.fontSize = 15;
    }

    public override void OnInspectorGUI()
    {
        PlayersStats();
    }

    void PlayersStats()
    {

        EditorGUILayout.LabelField("Basic Stats", _labelStyle);

        EditorGUILayout.Space();
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.blue);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        _player.maxHealth = EditorGUILayout.FloatField("Life", _player.maxHealth);
        _player.invincibleTime = EditorGUILayout.FloatField("Inmotality", _player.invincibleTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _player.speed = EditorGUILayout.FloatField("Speed", _player.speed);
        _player.forceJump = EditorGUILayout.FloatField("Jump Force", _player.forceJump);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Crouch total speed");
        _player.crouchSpeedModifier = EditorGUILayout.Slider(_player.crouchSpeedModifier,0,1);

        EditorGUILayout.Space();
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.blue);
        EditorGUILayout.Space();


        _showFoldout = EditorGUILayout.Foldout(_showFoldout,""); 
        if (_showFoldout)
        {
            EditorGUILayout.LabelField("Advance Stats", _labelStyle);

            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.red);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Floors Layers");

            LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_player.validFloorLayers), InternalEditorUtility.layers);
            _player.validFloorLayers = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Crouch Layers");

            LayerMask tempMask2 = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_player.crouchCheckLayerMask), InternalEditorUtility.layers);
            _player.crouchCheckLayerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

            EditorGUILayout.EndHorizontal();

            _player.groundedTreshold = EditorGUILayout.FloatField("Grounded Treshold", _player.groundedTreshold);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Coyotes Stats",_MiniCenter);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            _player.coyoteDuration = EditorGUILayout.FloatField("Duration", _player.coyoteDuration);
            _player.coyoteSupressionTime = EditorGUILayout.FloatField("Suppression Time", _player.coyoteSupressionTime);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(2, 10), Color.red);
            EditorGUILayout.Space();
        }
    }
}
