using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

public class LimitsWalls : EditorWindow
{
    AssetsSpawner _spawner;
    WallsScriptable _data;
    private GUIStyle _labelCenter;

    int intA;

    int Height;
    int Length;
    

    public static void OpenWindow()
    {
        var limit = (LimitsWalls)GetWindow(typeof(LimitsWalls));
    }

    private void OnEnable()
    {
        _labelCenter = new GUIStyle();
        _labelCenter.alignment = TextAnchor.MiddleCenter;
        _labelCenter.fontSize = 13;
    }

    private void OnGUI()
    {
        basicWall();
        RoomsCreator();
    }

    public void basicWall()
    {
        _spawner = (AssetsSpawner)EditorGUILayout.ObjectField(_spawner, typeof(AssetsSpawner), true);

        if (_spawner == null) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Wall Type ");
        intA = EditorGUILayout.IntSlider(intA, 1, _spawner.techo.Length);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Select Position ");

        EditorGUILayout.Space();
        var Up = GUILayout.Button("Up");
        if (Up)
            _spawner.SpawnWallLimitUp(intA);

        EditorGUILayout.BeginHorizontal();
        var Left = GUILayout.Button("Left");
        if (Left)
            _spawner.SpawnWallLimitLeft(intA);
        var Right = GUILayout.Button("Right");
        if (Right)
            _spawner.SpawnWallLimitRight(intA);
        EditorGUILayout.EndHorizontal();

        var Down = GUILayout.Button("Down");
        if (Down)
            _spawner.SpawnWallLimitDown(intA);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        var UpLeft = GUILayout.Button("Up Left");
        if (UpLeft)
            _spawner.SpawnWallLimitUpLeft(intA);
        var UpRight = GUILayout.Button("Up Right");
        if (UpRight)
            _spawner.SpawnWallLimitUpRight(intA);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        var DownLeft = GUILayout.Button("Down Left");
        if (DownLeft)
            _spawner.SpawnWallLimitDownLeft(intA);
        var DownRight = GUILayout.Button("Down Right");
        if (DownRight)
            _spawner.SpawnWallLimitDownRight(intA);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
    }

    void RoomsCreator()
    {

        if (_spawner == null) return;

        EditorGUILayout.LabelField("Large Wall", _labelCenter);

        Height = EditorGUILayout.IntField("Height", Height);
        Length = EditorGUILayout.IntField("Length", Length);

        var Down = GUILayout.Button("Create");

        if (Down)
            for (int i = 0; i <Length; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var go = new GameObject();
                    go.transform.position = new Vector3(i, j, 0);
                    _spawner.CreateLimitWalls(go, intA);
                    UnityEngine.Object.DestroyImmediate(go);
                }
            }

        EditorGUILayout.Space();

        if (GUILayout.Button("Create ScriptableObject"))
        {
            var ss = ScriptableObject.CreateInstance<WallsScriptable>();
            ss.Height = Height;
            ss.Length = Length;
            AssetDatabase.CreateAsset(ss, AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/" + Height + "X" + Length + "  Wall" + ".asset"));
            EditorUtility.SetDirty(ss);
        }
        EditorGUILayout.Space();

        _data = (WallsScriptable)EditorGUILayout.ObjectField(_data, typeof(WallsScriptable), true);
        if (_data != null)
        {
            Height = _data.Height;
            Length = _data.Length;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Deselected ScriptableObject"))
        {
            _data = null;
        }

        EditorGUILayout.Space();EditorGUILayout.Space();

        if (GUILayout.Button("Delete ScriptableObject"))
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_data));
            UpdateDatabase();
        }
    }

    public void UpdateDatabase()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
