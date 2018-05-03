using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLevelEditor
{

    public class OptionsWindow : EditorWindow 
    {
        bool _foldoutGameType = true;
        bool _foldoutPath = true;

        List<string> _gameTypes;
       string _configurationPath;
        bool _initialized = false;
        [MenuItem ("LevelEditor/Options")]
        public static void ShowOptionsWindow()
        {
            EditorWindow.GetWindow(typeof(OptionsWindow),true, "Options");
        }

        void OnInspectorUpdate()
        {
            _gameTypes = LevelEditorInfo.GetGameTypes();
            _initialized = true;
        }
        void OnGUI ()
        {
            if (!_initialized)
            {
                return;
            }

            _foldoutGameType =  EditorGUILayout.Foldout(_foldoutGameType,"选择游戏");
            if(_foldoutGameType)
            {
                EditorGUILayout.BeginVertical();

                foreach (var type in _gameTypes)
                {
                    bool isChosen = EditorPrefs.GetString(LevelEditorInfo.GAME_TYPE,"") == type;
                    isChosen = EditorGUILayout.Toggle(type, isChosen);
                    if(isChosen)
                    {
                        EditorPrefs.SetString(LevelEditorInfo.GAME_TYPE, type);
                    }

                }

                EditorGUILayout.EndVertical();
            }

            _foldoutPath = EditorGUILayout.Foldout(_foldoutPath, "设定配置文件夹(Configuration)");
            if (_foldoutPath)
            {
               _configurationPath = EditorPrefs.GetString(LevelEditorInfo.KEY_CONFIGURATION, Application.dataPath+"/"+ LevelEditorInfo.PATH_DEFAULT_CONFIG);
               EditorGUILayout.BeginHorizontal();

               _configurationPath = EditorGUILayout.TextField("",_configurationPath,GUILayout.MinWidth(400));
               if(GUILayout.Button("浏览"))
               {
                    _configurationPath = EditorUtility.OpenFolderPanel("选择配置文件夹", _configurationPath,"");
                    EditorPrefs.SetString(LevelEditorInfo.KEY_CONFIGURATION, _configurationPath);
               }
               
               
               EditorGUILayout.EndHorizontal();

               
            }




        }
    }
}
