using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;

[CustomEditor(typeof(GreenEnemy))]
public class GreenEnemyInspector : Editor
{
    GreenEnemy enemy;
    //bool originalInpector;
    AnimBool showInspector;
    AnimBool useParabola;
    public SerializedProperty teamProper;
    public SerializedProperty vidaActual;
    public SerializedProperty layerDetection;

    private void OnEnable()
    {
       enemy = (GreenEnemy)target;
        teamProper = serializedObject.FindProperty("myTeam");
        vidaActual = serializedObject.FindProperty("health");
        layerDetection = serializedObject.FindProperty("gameAreaMask");
        showInspector = new AnimBool(false);
        showInspector.valueChanged.AddListener(Repaint);
        useParabola = new AnimBool(false);
        useParabola.valueChanged.AddListener(Repaint);
    }

   /* public override void OnInspectorGUI()
    { 
       /* showInspector.target = EditorGUILayout.ToggleLeft("Mostrar Inspector Original: ", showInspector.target);
        if (EditorGUILayout.BeginFadeGroup(showInspector.faded))
        {
            EditorGUI.indentLevel++;

            DrawDefaultInspector();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
        if (showInspector.target) return;

        ShowNewInspector();
    }*/

    void ShowNewInspector()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        GUILayout.Label("Variables Principales");
        EditorGUILayout.Space();

        #region Team
        GUILayout.Label("Team", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.ObjectField(teamProper, new GUIContent("My Team: "));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        #endregion

        #region Vida
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
        GUILayout.Label("Jumping Pad", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        enemy.forceJump = EditorGUILayout.FloatField("Fuerza de Salto: ", enemy.forceJump);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        #endregion

        #region Shoting
        GUILayout.Label("Shoting", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.BeginHorizontal();
            {


                bool parabolaTemp = EditorGUILayout.Toggle("Usa Parabola: ", useParabola.target);
                if (parabolaTemp != enemy.useParabola)
                {
                    useParabola.target = parabolaTemp;
                    enemy.useParabola = parabolaTemp;
                    Undo.RecordObject(enemy, "Enemy Parabola Bool");
                }
            }
            EditorGUILayout.EndHorizontal();

            enemy.useParabola = useParabola.target;

            // enemy.shootWithGravity = (enemy.useParabola) ? false : true;


            if (EditorGUILayout.BeginFadeGroup(useParabola.faded))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        float newValor = EditorGUILayout.Slider("Altura Parabola", enemy.altBullet, -5, 5);
                        if (newValor != enemy.altBullet)
                        {
                            enemy.altBullet = newValor;
                            Undo.RecordObject(enemy, "Enemy Parabola Change");
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();


                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();

        }
        EditorGUILayout.EndVertical();
        #endregion



    }
}
