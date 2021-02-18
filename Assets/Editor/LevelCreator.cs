using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;
using UnityEditorInternal;


[ExecuteInEditMode]
public class LevelCreator : EditorWindow
{
    GameObject _pivot;
    AssetsSpawner _spawner;
    private GUIStyle _labelCenter;

    int intA;
    int intB;
    int intC;

    [MenuItem("Custom Tools/Level Creator")]
    public static void OpenWindow() 
    {
        LevelCreator myWindow = (LevelCreator)GetWindow(typeof(LevelCreator));
        myWindow.wantsMouseMove = true;
        myWindow.Show();
    }

    private void OnEnable()
    {
        _labelCenter = new GUIStyle();
        _labelCenter.alignment = TextAnchor.MiddleCenter;
        _labelCenter.fontSize = 13;
    }

    private void OnGUI()
    {
        Pivot();
        LevelAssets();
        LimitWalls();
    }

    void Pivot()
    {
        EditorGUILayout.LabelField("Pivot: ",_labelCenter);

        

        _pivot = (GameObject)EditorGUILayout.ObjectField(_pivot, typeof(GameObject), true);
        _spawner = (AssetsSpawner)EditorGUILayout.ObjectField(_spawner, typeof(AssetsSpawner), true);


        EditorGUILayout.HelpBox("Put the prefab Pivot v in boths", MessageType.Info);

        if (_spawner == null) return;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Move the pivot ",_labelCenter);

        EditorGUILayout.Space();

        var Up = GUILayout.Button("Up");
        if (Up)
            _pivot.transform.position = new Vector3(_pivot.transform.position.x, _pivot.transform.position.y + 2, _pivot.transform.position.z);

        EditorGUILayout.BeginHorizontal();
        var Left = GUILayout.Button("Left");
        if (Left)
            _pivot.transform.position = new Vector3(_pivot.transform.position.x - 2, _pivot.transform.position.y, _pivot.transform.position.z);
        var Right = GUILayout.Button("Right");
        if (Right)
            _pivot.transform.position = new Vector3(_pivot.transform.position.x + 2, _pivot.transform.position.y, _pivot.transform.position.z);
        EditorGUILayout.EndHorizontal();

        var Down = GUILayout.Button("Down");
        if (Down)
            _pivot.transform.position = new Vector3(_pivot.transform.position.x, _pivot.transform.position.y - 2, _pivot.transform.position.z);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

    }

    void LevelAssets()
    {
        if (_spawner == null) return;

        if (_pivot != null)
            _spawner.pivot = _pivot.transform;

        EditorGUILayout.LabelField("Select the assets ", _labelCenter);

        intA = EditorGUILayout.IntSlider(intA, 1, _spawner.techo.Length);
        var Up = GUILayout.Button("Roof");
        if (Up)
            _spawner.SpawnRoof(intA);

        EditorGUILayout.Space();

        intB = EditorGUILayout.IntSlider(intB, 1, _spawner.pared.Length);

        EditorGUILayout.BeginHorizontal();
        var Left = GUILayout.Button("Wall Left");
        if (Left)
            _spawner.SpawnWallLeft(intB);


        var Right = GUILayout.Button("Wall Right");
        if (Right)
            _spawner.SpawnWallRight(intB);
        EditorGUILayout.EndHorizontal();

        var Back = GUILayout.Button("Wall Bakground");
        if (Back)
            _spawner.SpawnWallBack(intB);

        EditorGUILayout.Space();

        intC = EditorGUILayout.IntSlider(intC, 1, _spawner.piso.Length);
        var Down = GUILayout.Button("Floor");
        if (Down)
            _spawner.SpawnFloor(intC);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
    void LimitWalls()
    {
        if (GUILayout.Button("Limit Walls",GUILayout.Height(30)))
        {
            //ver dentro de MoveAlongWindow para explicacion del por que almacenamos este parámetro aca
            LimitsWalls.OpenWindow();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Rooms", GUILayout.Height(30)))
        {
            //ver dentro de MoveAlongWindow para explicacion del por que almacenamos este parámetro aca
            RoomCreator.OpenWindow();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

}
