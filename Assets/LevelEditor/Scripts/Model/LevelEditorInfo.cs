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
        public const string EDITOR_CONFIG_ROOT = "LevelEditorConfigs/";
        public const string PATH_DEFAULT_CONFIG = "Assets/Resources/Configuration/";
        public const string FILE_META = "LevelEditorMeta";
        public const string FILE_GAME_UNIQUE = "GameUnique";
        public const string FILE_GAME_CONFIG = "game";
        //field names 
        public const string FIELD_GAMES = "games";
        public const string FIELD_BOARD_WIDTH = "boardwidth";
        public const string FIELD_BOARD_HEIGHT = "boardheight";
        public const string FIELD_LEVELNUM_TO_TYPE = "num_to_leveltype";
        public const string FIELD_SORTLEVELNUM_BEFORE = "sort_levels_before_this_num";


        
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
        public SortedDictionary<int, string> LevelNumToLevelType{get;private set;}
        public int SortLevelBeforeThisNum { get; private set; }

        //different between games
        public  int BoardWidth { get; private set; }
        public  int BoardHeight { get; private set; }
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
            
            string gameSpecificPath = EDITOR_CONFIG_ROOT + WhichGame + "/"+FILE_GAME_UNIQUE;

            UpdateGameUniqueData(gameSpecificPath);

            //other game unique data
            gameConfig = new GameConfig();
            var gameConfigNode = LevelEditorUtils.JSONNodeFromFile(ConfigurationFolderPath + FILE_GAME_CONFIG);
            gameConfig.Update(gameConfigNode);
            
            UpdateDefault();
        }

        void UpdateGameUniqueData(string gameSpecificPath)
        {
            var node = LevelEditorUtils.JSONNodeFromFile(gameSpecificPath);
        
            BoardWidth = node.GetInt(FIELD_BOARD_WIDTH);
            BoardHeight = node.GetInt(FIELD_BOARD_HEIGHT);
            
            //关卡类型与数字段的对应关系
            LevelNumToLevelType = new SortedDictionary<int,string>();
            var dic = node.GetDictionary(FIELD_LEVELNUM_TO_TYPE);
            foreach (var item in dic)
            {
                LevelNumToLevelType.Add(item.Value.AsInt(), item.Key);
            }

            SortLevelBeforeThisNum = node.GetInt(FIELD_SORTLEVELNUM_BEFORE);





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
            var node = LevelEditorUtils.JSONNodeFromFile(EDITOR_CONFIG_ROOT + FILE_META);
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
