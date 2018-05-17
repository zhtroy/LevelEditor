
using System;
using UnityEditor;
using UnityEngine;

public class GUIDtool : EditorWindow
{

    string _guid;
    [MenuItem("LevelEditor/GetAssetByGUID")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GUIDtool));
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _guid = EditorGUILayout.TextField(_guid);

        bool getbtn = GUILayout.Button("get");
        if (getbtn)
        {
            
             var a =AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(_guid));
            Selection.activeObject = a;
        }

        EditorGUILayout.EndVertical();
    }
}
