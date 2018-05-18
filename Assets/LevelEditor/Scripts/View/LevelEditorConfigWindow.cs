using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;
using System.Collections.Generic;


namespace CommonLevelEditor
{
    //要做的非常通用需要很多时间，没必要，做一个SMK和KIKI通用的就行
    public class LevelEditorConfigWindow : EditorWindow
    {
        public static LevelEditorConfigWindow instance;

        [MenuItem("LevelEditor/Configurations")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LevelEditorConfigWindow));
        }

       private void OnEnable()
        {
            instance = this;
        }

        private void OnDisable()
        {
            instance = null;
        }
        
        private void OnInspectorUpdate()
        {
            if (Application.isPlaying)
            {
                if (LevelListScrollerController.instance!=null)
                {
                    if (LevelListScrollerController.instance.CurrentLevel!=null && !_levelLoaded && EditingViewVisible())
                    {
                        LoadFromLevelData(LevelListScrollerController.instance.CurrentLevel);
                        _levelLoaded = true;
                    }
                }

                if (!EditingViewVisible())
                {
                    _levelLoaded = false;
                }
                Repaint();

            }
        }

        bool EditingViewVisible()
        {
            if (EditingView.instance != null)
            {
                return EditingView.instance.gameObject.activeInHierarchy;
            }
            return false;
        }

        bool _levelLoaded = false;

        bool _foldoutGeneral;
        bool _foldoutSupergems;
        bool _foldoutItems;
        bool _foldoutObjectives;
        bool _foldoutBoss;

        bool _initialized = true;

        
        LevelData _level;

        Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

        //Level Setting data
        //general
        string _levelName;
        int _levelNum;
        int _moves;
        CompanionTypeId _companionType;
        int _companionCharge;
        int _acsMin;
        int _acsMax;
        int[] _mastery;

        //super gem
        bool _superMergingEnabled;
        List<string> _preventItems;


        //从levelData 中读取settings 数据
        public void LoadFromLevelData(LevelData levelData)
        {
            _level = levelData;

            //单个数据
            _levelName = _level.name;
            _levelNum = _level.levelNum;
            _moves = _level.maxMoves;
            _companionType = _level.companionType;
            _companionCharge = _level.companionRequiredCharge;
            _acsMin = _level.acsMin;
            _acsMax = _level.acsMax;

           _mastery = _level.starThresholds.ToArray();
           
            _superMergingEnabled = _level.superMergingEnabled;
            _preventItems = new List<string>(_level.preventItems.ToArray());


            



            

        }

        public void SaveSettings()
        {
            if (_level==null)
            {
                return;
            }


        }

         void OnGUI()
         {

             if (EditingViewVisible())
             {

                _foldoutGeneral = EditorGUILayout.Foldout(_foldoutGeneral, "General", GetFoldOutStyle());

                if (_foldoutGeneral)
                {
                    GeneralSettings();
                }

                EditorGUILayout.Space();

                _foldoutSupergems = EditorGUILayout.Foldout(_foldoutSupergems, "Supergems", GetFoldOutStyle());

                if (_foldoutSupergems)
                {
                    SupergemSettings();
                }

                EditorGUILayout.Space();
                /*
                 _foldoutItems = EditorGUILayout.Foldout(_foldoutItems, "Items", GetFoldOutStyle());

                 if (_foldoutItems)
                 {
                     BoardItemsSettings();
                 }

                 EditorGUILayout.Space();

                 _foldoutObjectives = EditorGUILayout.Foldout(_foldoutObjectives, "Objectives", GetFoldOutStyle());

                 if (_foldoutObjectives)
                 {
                     ObjectivesSettings();
                 }

                 EditorGUILayout.Space();

                 _foldoutBoss = EditorGUILayout.Foldout(_foldoutBoss, "Options", GetFoldOutStyle());

                 if (_foldoutBoss)
                 {
                     BossSettings();
                 }

                 EditorGUILayout.Space();
                 */
            }
         }

         void GeneralSettings()
         {
             _levelName = EditorGUILayout.TextField("Level Name", _levelName);
             _levelNum = EditorGUILayout.IntField("Level Number", _levelNum );
            // _settingsView.__shouldClone = EditorGUILayout.Toggle("Clone Type", _settingsView.__shouldClone);
             _moves = EditorGUILayout.IntSlider("Moves", _moves, 0, 99);
             _companionType = (CompanionTypeId) EditorGUILayout.Popup("Companion Type",(int) (_companionType), Enum.GetNames(typeof(CompanionTypeId)));
             _companionCharge = EditorGUILayout.IntSlider("Companion Charge", _companionCharge, 0, 99);
            // _level.__energyCosts = EditorGUILayout.IntSlider("Energy Costs", _settingsView.__energyCosts, 0, 50);

             _acsMin= EditorGUILayout.IntSlider("ACS min", _acsMin, -50, 50);
             _acsMax = EditorGUILayout.IntSlider("ACS max", _acsMax, -50, 50);

             
             if (_mastery != null && _mastery.Length >= 3 )
             {
                 EditorGUILayout.BeginHorizontal();
                 EditorGUILayout.LabelField("Mastery");

                 _mastery[0] = EditorGUILayout.IntField(_mastery[0]);
                 _mastery[1] = EditorGUILayout.IntField(_mastery[1]);
                 _mastery[2] = EditorGUILayout.IntField(_mastery[2]);

                 //if (GUILayout.Button("Calculate"))
                 //{
                 //    GUIUtility.hotControl = 0;
                 //    GUIUtility.keyboardControl = 0;
                 //    _settingsView.OnCalculateMasteryValues();
                 //}

                 EditorGUILayout.EndHorizontal();
             }
         }

         void SupergemSettings()
         {
             _superMergingEnabled = EditorGUILayout.Toggle("Merging", _superMergingEnabled);
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.PreventItemCollection)
            {
                
                if (_preventItems.Contains(name))
                {
                    var select = EditorGUILayout.Toggle(name, false);
                    if (select == true)
                    {
                        _preventItems.Remove(name);
                    }
                }
                else
                {
                    var select = EditorGUILayout.Toggle(name, true);
                    if (select == false)
                    {
                        _preventItems.Add(name);
                    }
                }
                
            }
         }
        /*
         void BoardItemsSettings()
         {
             EditorGUILayout.LabelField("Colors");
             var colors = _settingsView.__colorOrderItems;
             DrawCounterItems(colors);

             EditorGUILayout.LabelField("Percentage");
             var dirtRatios = _settingsView.__dirtSpawnRatioItems;
             DrawCounterItems(dirtRatios);
             EditorGUILayout.LabelField("Min");
             var spawItemMin = _settingsView.__minSpawnRationItems;
             DrawCounterItems(spawItemMin);
             EditorGUILayout.LabelField("Max");
             var spawItemMax = _settingsView.__maxSpawnRationItems;
             DrawCounterItems(spawItemMax);
             EditorGUILayout.LabelField("Total Min");
             var totalSpawnItemMin = _settingsView.__totalMinSpawnItems;
             DrawCounterItems(totalSpawnItemMin);

             EditorGUILayout.LabelField("Total Max");
             var totalSpawnItemMax = _settingsView.__totalMaxSpawnItems;
             DrawCounterItems(totalSpawnItemMax);

             EditorGUILayout.LabelField("Climber");
             AddIcon("climber", 0);

             EditorGUILayout.Space();
             EditorGUILayout.Space();
             EditorGUILayout.Space();

             _settingsView.__climberAmount = EditorGUILayout.IntField(_settingsView.__climberAmount, GUILayout.MaxWidth(21));
         }

         void ObjectivesSettings()
         {

             _settingsView.minScore = EditorGUILayout.IntField("Score", _settingsView.minScore);

             var colors = _settingsView.__objectivesColors;

             DrawCounterItems(colors);

             _settingsView.__combineObjectives = EditorGUILayout.Toggle("Combine", _settingsView.__combineObjectives);

             var objectives = _settingsView.__objectivesItems;

             DrawCounterItems(objectives);

             _settingsView.movesOver = EditorGUILayout.Toggle("MovesOver", _settingsView.movesOver);

             // if(GUILayout.Button("Auto Fill", GUILayout.MaxWidth(150)))
             // {
             // 	GUIUtility.hotControl = 0;
             // 	GUIUtility.keyboardControl = 0;
             // 	_settingsView.OnCalculateObjectiveValues();
             // }

             // DrawSkillItems (_settingsView.__objectivesSkills);
         }

         void BossSettings()
         {
             _settingsView.__bossSelectedIndex = EditorGUILayout.Popup("Boss Type", _settingsView.__bossSelectedIndex, _settingsView.__bossSelectionList);
             _settingsView.__spawningSetIndex = EditorGUILayout.Popup("Spawning Set", _settingsView.__spawningSetIndex, _settingsView.__availableSpawningSetNames);
         }

         // void DrawSkillItems(List<LevelSettingsIcon> items)
         // {
         // 	int i = 0; 
         // 	for (; i<items.Count; i++) {
         // 		items[i]._amount = EditorGUILayout.IntField(items[i]._itemType,items[i]._amount);
         // 	}
         // }

         void DrawCounterItems(List<LevelSettingsIcon> items)
         {
             var count = 0;

             foreach (var item in items)
             {
                 AddIcon(item._itemType.ToLower(), count);
                 count++;
             }

             EditorGUILayout.Space();
             EditorGUILayout.Space();
             EditorGUILayout.Space();

             EditorGUILayout.BeginHorizontal();

             for (var i = 0; i < items.Count; i++)
             {
                 items[i]._amount = EditorGUILayout.IntField(items[i]._amount, GUILayout.MaxWidth(45));
             }

             EditorGUILayout.EndHorizontal();
         }

         void AddIcon(string name, int i)
         {
             Rect lastRect = GUILayoutUtility.GetLastRect();
             Material previewMaterial = EditorGUIUtility.Load("preview.mat") as Material;

             Texture texture;

             if (!_textures.TryGetValue(name + "_icon.png", out texture))
             {
                 texture = EditorGUIUtility.Load(name + "_icon.png") as Texture;
                 _textures.Add(name + "_icon.png", texture);
             }

             if (texture != null)
             {
                 EditorGUI.DrawPreviewTexture(new Rect(lastRect.x + (i * 50), lastRect.y + 20, 20, 20), texture, previewMaterial);
             }
             else
             {
                 Log.Warning("Level Editor :: Texture not found " + name);
             }
         }
         */
        GUIStyle GetFoldOutStyle()
         {
             GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
             myFoldoutStyle.fontStyle = FontStyle.Bold;
             myFoldoutStyle.fontSize = 14;
             myFoldoutStyle.active.textColor = Color.red;
             myFoldoutStyle.onActive.textColor = Color.red;
             return myFoldoutStyle;
         }
    }
}