using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class RoomCreator : EditorWindow
{
    AssetsSpawner _spawner;
    RoomScriptable _data;
    private GUIStyle _labelCenter;
    private bool _showFoldout;
   

    int intA;
    int intB;
    int intC;

    int Height;
    int Length;
    int Depth;

    bool backWall;
    bool leftWall;
    bool rightWall;
    bool floor;
    bool roof;

    public static void OpenWindow()
    {
        var limit = (RoomCreator)GetWindow(typeof(RoomCreator));
    }

    private void OnEnable()
    {
        _labelCenter = new GUIStyle();
        _labelCenter.alignment = TextAnchor.MiddleCenter;
        _labelCenter.fontSize = 13;
    }

    private void OnGUI()
    {
        Rooms();
    }

    public void Rooms()
    {
        _spawner = (AssetsSpawner)EditorGUILayout.ObjectField(_spawner, typeof(AssetsSpawner), true);

        if (_spawner == null) return;

        EditorGUILayout.LabelField("Rooms ", _labelCenter);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Wall Type ");
        intA = EditorGUILayout.IntSlider(intA, 1, _spawner.pared.Length);
        EditorGUILayout.LabelField("Floor Type ");
        intB = EditorGUILayout.IntSlider(intB, 1, _spawner.piso.Length);
        EditorGUILayout.LabelField("Roof Type ");
        intC = EditorGUILayout.IntSlider(intC, 1, _spawner.techo.Length);

        EditorGUILayout.Space(); EditorGUILayout.Space();

        Height = EditorGUILayout.IntField("Height", Height);
        Length = EditorGUILayout.IntField("Length", Length);
        Depth = EditorGUILayout.IntField("Depth", Depth);

        

        

        var Down = GUILayout.Button("Create");

        if (Down)
        {
            if (backWall)
                BackWall();
            if (floor)
                Floor();
            if (leftWall)
                LeftWall();
            if (rightWall)
                RightWall();
            if (roof)
                Roof();
        }

        _showFoldout = EditorGUILayout.Foldout(_showFoldout, "Room options");
        if (_showFoldout)
        {
            var Enable = GUILayout.Button("Enable All");
            if (Enable)
            {
                floor = true;
                leftWall = true;
                rightWall = true;
                backWall = true;
                roof = true;
            }
                floor = EditorGUILayout.Toggle("Floor", floor);
            leftWall = EditorGUILayout.Toggle("Left Wall", leftWall);
            rightWall = EditorGUILayout.Toggle("Right Wall", rightWall);
            backWall = EditorGUILayout.Toggle("Back Wall", backWall);
            roof = EditorGUILayout.Toggle("Roof", roof);

        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create ScriptableObject"))
        {
            var ss = ScriptableObject.CreateInstance<RoomScriptable>();
            ss.Height = Height;
            ss.Length = Length;
            ss.Depth = Depth;
            AssetDatabase.CreateAsset(ss, AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/" + Height + "X" + Length + "X" + Depth + "  Room" + ".asset"));
            EditorUtility.SetDirty(ss);
        }
        EditorGUILayout.Space();

        _data = (RoomScriptable)EditorGUILayout.ObjectField(_data, typeof(RoomScriptable), true);
        if (_data != null)
        {
            Height = _data.Height;
            Length = _data.Length;
            Depth = _data.Depth;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Deselected ScriptableObject"))
        {
            _data = null;
        }

        EditorGUILayout.Space(); EditorGUILayout.Space();

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

    void BackWall()
    {
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var go = new GameObject();
                go.transform.position = new Vector3(i, j, 0);
                _spawner.CreateBackWall(go, Depth, intA);
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
    void Floor()
    {
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                var go = new GameObject();
                go.transform.position = new Vector3(i, 0, j);
                _spawner.CreateFloor(go, intB);
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
    void LeftWall()
    {
        for (int i = 0; i < Depth; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var go = new GameObject();
                go.transform.position = new Vector3(0, j, i);
                _spawner.CreateLeftWall(go, intA);
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
    void RightWall()
    {
        for (int i = 0; i < Depth; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var go = new GameObject();
                go.transform.position = new Vector3(0, j, i);
                _spawner.CreateRightWall(go, Length, intA);
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
    void Roof()
    {
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                var go = new GameObject();
                go.transform.position = new Vector3(i, 0, j);
                _spawner.CreateRoof(go, Height, intC);
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
}
