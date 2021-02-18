using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssetsSpawner))]
public class PivotScriptable : Editor
{
    AssetsSpawner _assets;
    private void OnEnable()
    {
        _assets = (AssetsSpawner)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        if (GUILayout.Button("Create ScriptableObject"))
        {
            //ScriptableObjectUtility.CreateAsset<CanonScriptableObject>();
            var ss = ScriptableObject.CreateInstance<AssetScriptable>();
            ss.piso = _assets.piso;
            ss.pared = _assets.pared;
            ss.techo = _assets.techo;
            AssetDatabase.CreateAsset(ss, AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/" + "Pivot Textures" + ".asset"));
            //AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/" + typeof(CanonEditor).ToString() + ".asset");
            EditorUtility.SetDirty(ss);

        }
        EditorGUILayout.Space();

        _assets.data = (AssetScriptable)EditorGUILayout.ObjectField(_assets.data, typeof(AssetScriptable), true);
        if (_assets.data != null)
        {
            _assets.piso = _assets.data.piso;
            _assets.pared = _assets.data.pared;
            _assets.techo = _assets.data.techo;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Deselected ScriptableObject"))
        {
            _assets.data = null;
        }

        EditorGUILayout.Space(); EditorGUILayout.Space();

        if (GUILayout.Button("Delete ScriptableObject"))
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_assets.data));
            UpdateDatabase();
        }

    }
    public void UpdateDatabase()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
