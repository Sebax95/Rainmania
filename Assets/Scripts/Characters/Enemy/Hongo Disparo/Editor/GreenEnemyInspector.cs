using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using System;
/*
[CustomEditor(typeof(MushroomEnemy))]
public class GreenEnemyInspector : Editor
{
    MushroomEnemy enemy;
    //bool originalInpector;
    AnimBool showInspector;
    AnimBool useParabola;
    public SerializedProperty teamProper;
    public SerializedProperty vidaActual;
    public SerializedProperty layerDetection;
    public SerializedObject baseEnemy;


    private void OnEnable()
    {
        enemy = target as MushroomEnemy;
        teamProper = serializedObject.FindProperty("myTeam");
        vidaActual = serializedObject.FindProperty("health");
        layerDetection = serializedObject.FindProperty("gameAreaMask");
        baseEnemy = new SerializedObject(enemy);
        showInspector = new AnimBool(false);
        showInspector.valueChanged.AddListener(Repaint);
        useParabola = new AnimBool(false);
        useParabola.valueChanged.AddListener(Repaint);
    }

    /*public override void OnInspectorGUI()
    {
        baseEnemy.Update();
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        showInspector.target = EditorGUILayout.ToggleLeft("Mostrar Inspector Original: ", showInspector.target);
        if (EditorGUILayout.BeginFadeGroup(showInspector.faded))
        {
            EditorGUI.indentLevel++;

            DrawDefaultInspector();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
        if (showInspector.target) return;

        ShowNewInspector();
        GUI.color = new Color(56, 56, 56);
        /*if (EditorGUI.EndChangeCheck())
        {
        if (GUILayout.Button("Guardar Datos"))
        {
            EditorUtility.SetDirty(enemy);
            Debug.Log(EditorUtility.IsDirty(enemy));
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            baseEnemy.ApplyModifiedProperties();
            Undo.RecordObject(enemy, "Variables Modificadas");
            EditorSceneManager.SaveOpenScenes();
            //baseEnemy.ApplyModifiedPropertiesWithoutUndo();
        }

        //}
    }

    void ShowNewInspector()
    {


        EditorGUILayout.Space();
        GUILayout.Label("Variables Principales");
        EditorGUILayout.Space();

        #region Team
        GUI.color = Color.magenta;
        GUILayout.Label("Team", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.ObjectField(teamProper, new GUIContent("My Team: "));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        #endregion

        #region Vida
        GUI.color = Color.red;
        GUILayout.Label("Vida", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        if (Application.isPlaying)
        {
            GUILayout.Label("Max Health: ");
            EditorGUILayout.HelpBox("No Modificable en PlayMode", MessageType.Warning);
        }
        else
            enemy.maxHealth = EditorGUILayout.FloatField("Max Health: ", enemy.maxHealth);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        vidaActual.floatValue = EditorGUILayout.FloatField("Vida Actual: ", vidaActual.floatValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        #endregion

        #region Line Of Sight
        GUI.color = Color.yellow;
        GUILayout.Label("Line Of Sight", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        enemy.viewDistance = EditorGUILayout.FloatField("Distancia: ", enemy.viewDistance);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        enemy.viewAngle = EditorGUILayout.FloatField("Angulo: ", enemy.viewAngle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        // layerDetection. = EditorGUILayout.EnumFlagsField(layerDetection);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        enemy.showGizmos = EditorGUILayout.Toggle("Mostrar Gizmos: ", enemy.showGizmos);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        #endregion

        #region JumpingPad
        GUI.color = Color.cyan;
        GUILayout.Label("Jumping Pad", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        enemy.forceJump = EditorGUILayout.FloatField("Fuerza de Salto: ", enemy.forceJump);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        #endregion

        #region Shoting

        Parabola();

        #endregion
    }

    public void Parabola()
    {
        GUI.color = Color.green;
        GUILayout.Label("Shoting", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        {
            useParabola.target = EditorGUILayout.Toggle("Usa Parabola: ", useParabola.target);
            if (EditorGUILayout.BeginFadeGroup(useParabola.faded))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        enemy.useParabola = true;
                        enemy.shootWithGravity = (enemy.useParabola) ? true : false;
                        enemy.altBullet = EditorGUILayout.Slider("Altura Parabola", enemy.altBullet, -5, 5);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            if (!useParabola.target)
                enemy.useParabola = false;
        }
        EditorGUILayout.EndVertical();
    }
}*/
