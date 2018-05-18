using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Wooga.Foundation.Json;

namespace CommonLevelEditor
{
    public class LevelEditorInfo
    {
        public const string GAME_TYPE = "Level Editor Type";
        public const string KEY_CONFIGURATION = "Configuration Path";
        //file path  use camel naming
        public const string EDITOR_SPRITE = "Sprites/";
        public const string EDITOR_CONFIG_ROOT = "LevelEditorConfigs/";
        public const string PATH_DEFAULT_CONFIG = "Assets/Resources/Configuration/";
        public const string FILE_META = "LevelEditorMeta";
        public const string FILE_GAME_UNIQUE = "GameUnique";
        public const string FILE_LEVEL_SETTINGS = "LevelSettings";
        public const string FILE_GAME_ITEMS = "Items";
        public const string FILE_BRUSH = "Brush";
        public const string FILE_GAME_CONFIG = "game";
        //field names 
        public const string FIELD_COLLECTION = "collection";
        public const string FIELD_GAMES = "games";
        public const string FIELD_BOARD_WIDTH = "boardwidth";
        public const string FIELD_BOARD_HEIGHT = "boardheight";
        public const string FIELD_LEVELNUM_TO_TYPE = "num_to_level_filename";
        public const string FIELD_SORTLEVELNUM_BEFORE = "sort_levels_before_this_num";
        public const string FIELD_LEVELS_PER_FILE = "levels_per_file";
        public const string FIELD_LEVELS_NUM_ORIGIN = "level_num_origin";
        public const string FIELD_LAYERS = "layers";



        private static LevelEditorInfo _instance = null;
        public static LevelEditorInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelEditorInfo();
                }
                return _instance;
            }
            
        }

        #region Property
        //meta --different between games
        public string WhichGame{get; private set;}
        public List<string> GameTypes{get; private set;}
        public string ConfigurationFolderPath{get; private set;} 
        public string FullConfigurationFolderPath { get;  private set;}
       

        //GameUnique
        public  int BoardWidth { get; private set; }
        public  int BoardHeight { get; private set; }
        public  List<int> InvisibleBoardIndex { get; private set; }
        public SortedDictionary<int, string> LevelNumToLevelType { get; private set; }
        public int SortLevelBeforeThisNum { get; private set; }
        public int LevelsPerFile { get; private set; }
        public int LevelNumOrigin { get; private set; }

        public List<string> Layers { get; private set; }

        public Dictionary<string,BoardItem> DicBoardItem { get; private set; }
        private Dictionary<string, Sprite> _itemSprites;

        public LevelSettingConfig levelSettingConfig { get; private set; }

        //game config
        public GameConfig gameConfig { get; private set; }

        //default

        public Dictionary<string, object> DefaultValues { get; private set; }
        public Dictionary<string, Dictionary<string, LevelMatrix>> DefaultLayers { get; private set; }


        //json configs --different between games
      

        #endregion

        LevelEditorInfo()
        {
            LoadAllInfo();
        }

        public void LoadAllInfo()
        {
            UpdateMetaInfo();

            CheckWhichGame();
            

            UpdateGameUniqueData(EditorConfigPath + FILE_GAME_UNIQUE);

            UpdateLevelSettingsData(EditorConfigPath + FILE_LEVEL_SETTINGS);

            UpdateGameItems(EditorConfigPath + FILE_GAME_ITEMS);

            //other game unique data
            gameConfig = new GameConfig();
            var gameConfigNode = LevelEditorUtils.JSONNodeFromFileFullPath(FullConfigurationFolderPath + FILE_GAME_CONFIG);
            gameConfig.Update(gameConfigNode);
            
            UpdateDefault();
        }

        public string EditorConfigPath
        {
            get
            {
                return  WhichGame+ "/" + EDITOR_CONFIG_ROOT ;
            }
        }

        public Sprite GetItemSpriteByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            if (_itemSprites.ContainsKey(name))
            {
                return _itemSprites[name];
            }
            return null;
        }
        void UpdateGameItems(string itemsPath)
        {
            var node = LevelEditorUtils.JSONNodeFromFileResourcesPath(itemsPath);
            DicBoardItem = new Dictionary<string, BoardItem>();
            _itemSprites = new Dictionary<string, Sprite>();
            var collection = node.GetCollection("items");
            foreach (var data in collection)
            {
                BoardItem t = new BoardItem();
                t.Update(data);
                Sprite s = Resources.Load<Sprite>(WhichGame +"/Sprites/" + t.SpriteId);
                _itemSprites.Add(t.Name, s);
                DicBoardItem.Add(t.Name, t);
            }

        }

        void UpdateLevelSettingsData(string path)
        {
            var node = LevelEditorUtils.JSONNodeFromFileResourcesPath(path);
            if (node != null)
            {
                levelSettingConfig = new LevelSettingConfig();
                levelSettingConfig.Update(node);
            }
            
        }
        void UpdateGameUniqueData(string gameSpecificPath)
        {
            var node = LevelEditorUtils.JSONNodeFromFileResourcesPath(gameSpecificPath);
        
            BoardWidth = node.GetInt(FIELD_BOARD_WIDTH);
            BoardHeight = node.GetInt(FIELD_BOARD_HEIGHT);

            InvisibleBoardIndex = new List<int>();
            var invisible_board_index = node.GetCollection("invisible_board_index");

            foreach (var item in invisible_board_index)
            {
                InvisibleBoardIndex.Add(item.AsInt());
            }

            //关卡类型与数字段的对应关系
            LevelNumToLevelType = new SortedDictionary<int,string>();
            var dic = node.GetDictionary(FIELD_LEVELNUM_TO_TYPE);
            foreach (var item in dic)
            {
                LevelNumToLevelType.Add(item.Value.AsInt(), item.Key);
            }

            SortLevelBeforeThisNum = node.GetInt(FIELD_SORTLEVELNUM_BEFORE);

            LevelsPerFile = node.GetInt(FIELD_LEVELS_PER_FILE);

            LevelNumOrigin = node.GetInt(FIELD_LEVELS_NUM_ORIGIN);

            Layers = new List<string>();








    }
    void CheckWhichGame()
        {
            var gametype = EditorPrefs.GetString(GAME_TYPE,"Sumikko");
            if (GameTypes.Contains(gametype))
            {
                WhichGame = gametype;
            }
            else
            {
                Debug.LogError("no matching game type "+ gametype + " in LevelEditorMeta.json file");
                
            }
        }

        // read from LevelEditorMeta.json 
        void UpdateMetaInfo()
        {
            //读取所有游戏列表
            GameTypes = GetGameTypes();

            //游戏配置路径
            InitConfigurationPath();



        }

        void InitConfigurationPath()
        {
            string configPath = EditorPrefs.GetString(KEY_CONFIGURATION, "") + "/";
            FullConfigurationFolderPath = configPath;
            if (configPath == "")
            {
                Debug.LogError("游戏配置文件夹未设置，请在Options中设置");
            }

            if (configPath.IndexOf("Resources/") >= 0)
            {
                configPath = (configPath).Split(new[] { "Resources/" }, StringSplitOptions.RemoveEmptyEntries)[1];

            }

            ConfigurationFolderPath = configPath;
        }

        public static List<string> GetGameTypes()
        {
            List<string> gameTypes = new List<string>();
            var node = LevelEditorUtils.JSONNodeFromFileResourcesPath(FILE_META);
            var games = node.GetCollection(FIELD_GAMES);
            foreach(var item in games)
            {
                gameTypes.Add(item.AsString());
            }
            return gameTypes;
        }


        //在LevelData类中会用的默认值
        void UpdateDefault()
        {
            DefaultValues = new Dictionary<string, object>
                                {
                                    { LevelData.MAX_MOVES, 0 },
                                    { LevelData.SUPER_MERGING_ENABLED, true },
                                    { LevelData.BOSS_NAME, "0" },
                                    { LevelData.ACS_MIN, 0 },
                                    { LevelData.ACS_MAX, 0 }
                                };
            DefaultLayers = new Dictionary<string, Dictionary<string, LevelMatrix>>{
                { ConfigLayerId.FIELDS, new Dictionary<string, LevelMatrix>{
                        { ConfigLayerId.TYPES, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.HITPOINTS, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.OPTIONS, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.COLORS, new LevelMatrix(BoardWidth, BoardHeight)}
                    }
                },
                { ConfigLayerId.ITEMS, new Dictionary<string, LevelMatrix>{
                        { ConfigLayerId.TYPES, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.HITPOINTS, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.COLORS, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.OPTIONS, new LevelMatrix(BoardWidth, BoardHeight)},
                        { "connections", new LevelMatrix(BoardWidth, BoardHeight)}
                    }
                },
                { ConfigLayerId.COVERS, new Dictionary<string, LevelMatrix>{
                        { ConfigLayerId.TYPES, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.HITPOINTS, new LevelMatrix(BoardWidth, BoardHeight)},
                        { ConfigLayerId.OPTIONS, new LevelMatrix(BoardWidth, BoardHeight)},
                        { "connections", new LevelMatrix(BoardWidth, BoardHeight)}
                    }
                },
            };

        }
              
    }
}
