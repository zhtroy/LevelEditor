using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;
using System.Collections.Generic;


namespace CommonLevelEditor
{
    //要做的非常通用需要很多时间，没必要，做一个SMK和KIKI通用的就行

    class LevelSettingsIcon
    {
        public string itemType;

        public int amount;

        public LevelSettingsIcon(string type, int amount)
        {
            itemType = type;
            this.amount = amount;
        }
    }
    public class LevelEditorConfigWindow : EditorWindow
    {
        public static LevelEditorConfigWindow instance;

        const string NO_BOSS_STRING = "No boss";

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

        Vector2 scrollPos;
        bool _foldoutGeneral;
        bool _foldoutSupergems;
        bool _foldoutItems;
        bool _foldoutObjectives;
        bool _foldoutBoss;

        bool _initialized  = false;

        
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

        //Items
        List<LevelSettingsIcon> _colorOrder;
        List<LevelSettingsIcon> _itemSpawnChance;
        List<LevelSettingsIcon> _itemSpawnMin;
        List<LevelSettingsIcon> _itemSpawnMax;
        List<LevelSettingsIcon> _totalMin;
        List<LevelSettingsIcon> _totalMax;
        List<LevelSettingsIcon> _ensureItems;

        //Objectives
        List<LevelSettingsIcon> _objectives;
        bool _moveOver;
        List<LevelSettingsIcon> _objectiveColor;
        bool _combineObjective;
        int _combineTotal;

        //boss
        int _bossIndex;
        string[] _bossNames;
        int _spawningSetIndex;
        string[] _spawningSets;


        public void SaveSettingsToLevelData()
        {
            //单个数据

            _level.name = _levelName;
            _level.levelNum = _levelNum;
            _level.maxMoves = _moves;
            _level.companionType = _companionType;
            _level.companionRequiredCharge = _companionCharge;
            _level.acsMax = _acsMax;
            _level.acsMin = _acsMin;
            _level.starThresholds = new List<int>(_mastery);

            //supergem

            _level.superMergingEnabled = _superMergingEnabled;
            _level.preventItems = new List<string>(_preventItems);
            //Items

            List<ColorId> colorOrderList = new List<ColorId>();
            for (int i = 0; i < 6; i++)
            {
                
                var item = _colorOrder.Find(x => x.amount == i);
                if (item !=null)
                {
                    colorOrderList.Add(LevelEditorUtils.ParseEnum<ColorId>(item.itemType)); 
                }
            }
            _level.colorSpawningOrder = colorOrderList;


            foreach (var item in _itemSpawnChance)
            {
                _level.itemSpawnPercentages[item.itemType] = item.amount;
            }
            

            foreach (var item in _itemSpawnMin)
            {
                _level.itemSpawnMin[item.itemType] = item.amount;
            }


            foreach (var item in _itemSpawnMax)
            {
                _level.itemSpawnMax[item.itemType] = item.amount;
            }


            foreach (var item in _totalMin)
            {
                _level.itemTotalSpawnMin[item.itemType] = item.amount;
            }


            foreach (var item in _totalMax)
            {
                _level.itemTotalSpawnMax[item.itemType] = item.amount;
            }


            foreach (var item in _ensureItems)
            {
                _level.ensureItems[item.itemType] = item.amount;
            }


            List<ColorWinningCondition> colorWinningCondition = new List<ColorWinningCondition>();
            if (_combineObjective)
            {
                ColorWinningCondition condition = new ColorWinningCondition();
                condition.colors = new List<ColorId>();
                foreach (var item in _objectiveColor)
                {
                    if (item.amount!=0)
                    {
                        condition.colors.Add(LevelEditorUtils.ParseEnum<ColorId>(item.itemType));
                    }
                }
                if (condition.colors.Count!=0)
                {
                    condition.amount = _combineTotal;
                    colorWinningCondition.Add(condition);
                }
            }
            else
            {
                foreach (var item in _objectiveColor)
                {
                    if (item.amount!=0)
                    {
                        ColorWinningCondition condition = new ColorWinningCondition();
                        condition.colors = new List<ColorId>();
                        condition.colors.Add(LevelEditorUtils.ParseEnum<ColorId>(item.itemType));
                        condition.amount = item.amount;
                        colorWinningCondition.Add(condition);
                    }
                }
            }
            _level.colorWinningConditions = colorWinningCondition;

            foreach (var item in _objectives)
            {
                if (item.amount!=0)
                {
                    _level.typeWinningConditions[item.itemType] = item.amount;
                }
               
            }

            _level.movesOver = _moveOver;

            //boss

            _level.bossName = _bossNames[_bossIndex] == NO_BOSS_STRING ? "0" : _bossNames[_bossIndex];


            if (_spawningSetIndex<0)
            {
                _level.spawningSetName = "";
            }
            else
            {
                _level.spawningSetName = _spawningSets[_spawningSetIndex];
            }
            













        }
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
           
            //supergem
            _superMergingEnabled = _level.superMergingEnabled;
            _preventItems = new List<string>(_level.preventItems.ToArray());

            //Items
            _colorOrder = new List<LevelSettingsIcon>();
            for (int i = 0; i < 6; i++)
            {
                ColorId color = (ColorId)i;

                _colorOrder.Add(new LevelSettingsIcon(color.ToString(),-1));
            }
            int sequence = 0;
            foreach (var color in _level.colorSpawningOrder)
            {
                _colorOrder[(int)color].amount = sequence;
                sequence++;
            }

            _itemSpawnChance = new List<LevelSettingsIcon>();
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.SpawnChanceCollection)
            {
                int value;
                if (_level.itemSpawnPercentages.TryGetValue(name,out value))
                {
                    _itemSpawnChance.Add(new LevelSettingsIcon(name, value));
                }
            }

            _itemSpawnMin = new List<LevelSettingsIcon>();
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.SpawnChanceCollection)
            {
                int value=0;
                _level.itemSpawnMin.TryGetValue(name, out value);
              
                _itemSpawnMin.Add(new LevelSettingsIcon(name, value));
            
            }

            _itemSpawnMax = new List<LevelSettingsIcon>();
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.SpawnChanceCollection)
            {
                int value=0;
                _level.itemSpawnMax.TryGetValue(name, out value);
                _itemSpawnMax.Add(new LevelSettingsIcon(name, value));
               
            }

            _totalMin = new List<LevelSettingsIcon>();
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.TotalChanceCollection)
            {
                int value=0;
                _level.itemTotalSpawnMin.TryGetValue(name, out value);
               
                _totalMin.Add(new LevelSettingsIcon(name, value));
              
            }

            _totalMax = new List<LevelSettingsIcon>();
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.TotalChanceCollection)
            {
                int value=0;
               _level.itemTotalSpawnMax.TryGetValue(name, out value);
                
                _totalMax.Add(new LevelSettingsIcon(name, value));
               
            }

            _ensureItems = new List<LevelSettingsIcon>();
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.EnsureItemCollection)
            {
                int value = 0;
                _level.ensureItems.TryGetValue(name, out value);
                _ensureItems.Add(new LevelSettingsIcon(name, value));
             
            }

            _objectiveColor = new List<LevelSettingsIcon>();
            foreach (var item in ColorUtils.allColors)
            {
                _objectiveColor.Add(new LevelSettingsIcon((item).ToString(), 0));
            }

            foreach (var condition in _level.colorWinningConditions)
            {
                var condColors = condition.colors;
                if (condColors.Count>1)
                {
                    _combineObjective = true;
                    foreach (var color in condColors)
                    {
                        int idx = ColorUtils.allColors.IndexOf(color);
                        _objectiveColor[idx].amount = 1;
                    }
                    _combineTotal = condition.amount;
                    break;
                }
                else
                {
                    int idx = ColorUtils.allColors.IndexOf(condColors[0]);
                    _objectiveColor[idx].amount = condition.amount;
                }
                   
            }

            _objectives = new List<LevelSettingsIcon>();
            foreach (var name in LevelEditorInfo.Instance.levelSettingConfig.ObjectiveCollection)
            {
                int value;
                _level.typeWinningConditions.TryGetValue(name, out value);

                _objectives.Add(new LevelSettingsIcon(name, value));
           
            }

            _moveOver = _level.movesOver;


            //boss
            List<string> tempboss = new List<string>() { NO_BOSS_STRING };
            
            foreach (var boss in LevelEditorInfo.Instance.gameConfig.bossNames)
            {
                tempboss.Add(boss);
            }
            
             if (!String.IsNullOrEmpty( _level.bossName))
            {
                _bossIndex = tempboss.IndexOf(_level.bossName);
                if (_bossIndex==-1)
                {
                    _bossIndex = 0;
                }
            }
            else
            {
                _bossIndex = 0;
            }


            _bossNames = tempboss.ToArray();
            _spawningSets = LevelEditorInfo.Instance.gameConfig.availableSpawningSets.ToArray();
            _spawningSetIndex = -1;
            if (!String.IsNullOrEmpty( _level.spawningSetName))
            {
                _spawningSetIndex = LevelEditorInfo.Instance.gameConfig.availableSpawningSets.IndexOf(_level.spawningSetName);
                
            }
            















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

             if (EditingViewVisible()&& _levelLoaded)
             {
                scrollPos= EditorGUILayout.BeginScrollView(scrollPos);

                _foldoutGeneral = EditorGUILayout.Foldout(_foldoutGeneral, "General", GetFoldOutStyle());

                if (_foldoutGeneral)
                {
                    EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
                    GeneralSettings();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();

                _foldoutSupergems = EditorGUILayout.Foldout(_foldoutSupergems, "Supergems", GetFoldOutStyle());

                if (_foldoutSupergems)
                {
                    SupergemSettings();
                }

                EditorGUILayout.Space();
            
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
                    EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
                    BossSettings();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();

                EditorGUILayout.EndScrollView();
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

        void AddIcon(string name, int i)
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Material previewMaterial = EditorGUIUtility.Load("preview.mat") as Material;

            Sprite sp = LevelEditorInfo.Instance.GetItemSpriteByName(name);

            Texture texture = null;
            if (sp!=null)
            {
                texture = sp.texture;
            }

            if (texture != null)
            {
                GUI.DrawTexture(new Rect(lastRect.x + (i * 50), lastRect.y + 20, 30, 30), texture);
            }
            else
            {
                Debug.LogWarning("Level Editor :: Texture not found " + name);
            }
        }

        void DrawCounterItems(List<LevelSettingsIcon> items)
        {
            var count = 0;

            foreach (var item in items)
            {
                AddIcon( item.itemType, count);
                count++;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();



            EditorGUILayout.BeginHorizontal();

            for (var i = 0; i < items.Count; i++)
            {
                items[i].amount = EditorGUILayout.IntField(items[i].amount, GUILayout.MaxWidth(45));
            }

            EditorGUILayout.EndHorizontal();
        }
        void BoardItemsSettings()
         {
             EditorGUILayout.LabelField("宝石生成顺序");
                
             DrawCounterItems(_colorOrder);

             EditorGUILayout.LabelField("Item Spawn Chance");
             
             DrawCounterItems(_itemSpawnChance);
           
             EditorGUILayout.LabelField("Item Spawn Min");

             DrawCounterItems(_itemSpawnMin);
       
             EditorGUILayout.LabelField("Item Spawn Max");
             
             DrawCounterItems(_itemSpawnMax);

            if (_totalMin.Count!=0)
            {
                EditorGUILayout.LabelField("Total Min");

                DrawCounterItems(_totalMin);
            }

            if (_totalMax.Count != 0)
            {
                EditorGUILayout.LabelField("Total Max");

                DrawCounterItems(_totalMax);
            }

            if (_ensureItems.Count!=0)
            {
                EditorGUILayout.LabelField("确保生成物品");

                DrawCounterItems(_ensureItems);
            }



        }

      
         void ObjectivesSettings()
         {

             //_settingsView.minScore = EditorGUILayout.IntField("Score", _settingsView.minScore);

             
            

            DrawCounterItems(_objectiveColor);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(200));

            _combineObjective = EditorGUILayout.Toggle("合并颜色", _combineObjective);

            if (_combineObjective)
            {
                _combineTotal = EditorGUILayout.IntField("总数", _combineTotal);
            }
            EditorGUILayout.EndHorizontal();
                      
             DrawCounterItems(_objectives);

            _moveOver = EditorGUILayout.Toggle("MovesOver", _moveOver);

            

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
             _bossIndex = EditorGUILayout.Popup("Boss Type", _bossIndex, _bossNames);
             _spawningSetIndex = EditorGUILayout.Popup("Spawning Set", _spawningSetIndex, _spawningSets);
        }

        // void DrawSkillItems(List<LevelSettingsIcon> items)
        // {
        // 	int i = 0; 
        // 	for (; i<items.Count; i++) {
        // 		items[i]._amount = EditorGUILayout.IntField(items[i]._itemType,items[i]._amount);
        // 	}
        // }




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