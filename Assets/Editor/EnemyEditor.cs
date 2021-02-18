using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 
using UnityEditor.AnimatedValues; 
using System;

[CustomEditor(typeof(EnemyView))]
public class EnemyEditor : Editor
{
    EnemyView _enemyE;
    private bool _showFoldout;
    private GUIStyle _labelStyle;

    private void OnEnable()
    {
        _enemyE = (EnemyView)target;

        _labelStyle = new GUIStyle();
        _labelStyle.fontStyle = FontStyle.Bold;
        _labelStyle.alignment = TextAnchor.MiddleCenter;
        _labelStyle.fontSize = 20;
    }

    public override void OnInspectorGUI()
    {
        Parameters();
    }

    void Parameters()
    {
        EditorGUILayout.LabelField("Animator Parameters", _labelStyle);
        EditorGUILayout.Space();
        for (int i = 0; i < _enemyE.triggersNames.Length; i++)
        {
            _enemyE.triggersNames[i] = EditorGUILayout.TextField(_enemyE.triggersNames[i]);
           
        }

        _showFoldout = EditorGUILayout.Foldout(_showFoldout, "IsDead");
        if (_showFoldout)
        {
            EditorGUILayout.HelpBox("Solo si posee animacion de muerte", MessageType.Info);
            for (int i = 0; i < _enemyE.boolsNames.Length; i++)
                _enemyE.boolsNames[0] = EditorGUILayout.TextField(_enemyE.boolsNames[0]);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Sounds", _labelStyle);
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("In order: Die, Hit, Shoot & Jump", MessageType.Warning);
        for (int i = 0; i < _enemyE.audios.Count; i++)
        {
            _enemyE.audios[i] = (AudioClip)EditorGUILayout.ObjectField(_enemyE.audios[i], typeof(AudioClip));
        }



    }
}
