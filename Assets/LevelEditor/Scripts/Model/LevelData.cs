using System;
using Wooga.Foundation.Json;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace CommonLevelEditor
{
    //纯关卡数据部分
	public partial class LevelData
	{

        #region public events
        //params: layername,  index, data
        public event Action<string, int, string> onDataChanged;
        #endregion
        #region property
        public int CellNum
        {
            get
            {
                return width* height;
            }
        }

        #endregion

        readonly static string[] TOTAL_ITEM_TYPES =
            {
            ItemTypeId.Key.ToString(),
        };


		readonly static string[] ITEM_TYPES =
			{
				ItemTypeId.Dirt + "_1",
				ItemTypeId.Dirt + "_2",
				ItemTypeId.Dirt + "_3",
				ItemTypeId.Dirt + "_4",
				ItemTypeId.Dirt + "_5",
                ItemTypeId.CannonGem.ToString(),
				ItemTypeId.Question.ToString(),
                ItemTypeId.Key + "_0",
                ItemTypeId.Key + "_1" ,
                ItemTypeId.Key + "_2" ,
                ItemTypeId.Key + "_3" ,
                ItemTypeId.Key + "_4" ,
                ItemTypeId.Key + "_5" 
            };

       /*public readonly static Dictionary<string, int> ItemType_CID_Map = new Dictionary<string, int>()
        {
            { ItemTypeId.Dirt + "_1",CId.Dirt },
            { ItemTypeId.Dirt + "_2",CId.Dirt},
            {  ItemTypeId.Dirt + "_3",CId.Dirt},
            {  ItemTypeId.Dirt + "_4",CId.Dirt},
            {  ItemTypeId.Dirt + "_5",CId.Dirt},
            {  ItemTypeId.CannonGem.ToString(),CId.CannonShooting},
            {  ItemTypeId.Question.ToString(),CId.Question},
            { ItemTypeId.Key + "_0",CId.Key},
            { ItemTypeId.Key + "_1" ,CId.Key},
            {  ItemTypeId.Key + "_2" ,CId.Key},
            { ItemTypeId.Key + "_3" ,CId.Key},
            { ItemTypeId.Key + "_4" ,CId.Key},
            {  ItemTypeId.Key + "_5",CId.Key},
        };
        */

		public const int COLOR_CHANCE = 16;
		public const string ID = "id";
		public const string NAME = "name";
		public const string LEVEL_NUM = "level";
		public const string TYPE = "type";
		public const string WIDTH = "width";
		public const string HEIGHT = "height";
		public const string MAX_MOVES = "maxMoves";
		public const string SUPER_MERGING_ENABLED = "superMergingEnabled";
		public const string LAYERS = "layers";
		public const string CONDITIONS = "conditions";
		public const string COMPANION = "companion";
		public const string REQUIRED_CHARGE = "requiredCharge";
		public const string COLOR_SPAWN_PERCENTAGES = "colorSpawnPercentages";
		public const string ITEM_SPAWN_PERCENTAGES = "itemSpawnPercentages";
        public const string ITEM_SPAWN_MIN = "itemSpawnMin";
        public const string ITEM_SPAWN_MAX = "itemSpawnMax";
        public const string ITEM_TOTAL_SPAWN_MIN = "itemTotalSpawnMin";
        public const string ITEM_TOTAL_SPAWN_MAX = "itemTotalSpawnMax";
		public const string PREVENT_ITEMS = "preventItems";
		public const string ENSURE_ITEMS = "ensureItems";
		public const string SCORE = "score";
		public const string MOVESOVER = "movesover";
		public const string STARS = "stars";
		public const string ADAPTIVE_COLOR_SPAWNING_OFFSET = "adaptiveColorSpawningOffset";
		public const string ACS_MIN = "acsMin";
		public const string ACS_MAX = "acsMax";
		public const string COLOR_SPAWNING_ORDER = "colorSpawningOrder";
		public const string SPAWNING_SET_NAME = "spawingSetName";
		public const string BOSS_NAME = "bossName";
		public const string BOSS_HITPOINTS = "bossHitpoints";
		public const string BOSS_AMMO_TYPE = "bossAmmoType";
		public const string BOSS_AMMO_LAYERID = "bossAmmoLayerId";
		public const string BOSS_AMMO_INTERVAL = "bossAmmoInterval";
		public const string BOSS_AMMO_AMOUNT = "bossAmmoAmount";
		public const string BOSS_AMMO_HITPOINTS = "bossAmmoHitpoints";

		public string id { get; set; }

		public string name { get; set; }

		public int levelNum { get; set; }

		public int width { get; set; }

		public int height { get; set; }

		public int maxMoves { get; set; }

		public IDictionary<ItemTypeId, int> typeWinningConditions { get; set; }

		public IList<ColorWinningCondition> colorWinningConditions { get; set; }

		public Dictionary<CompanionTypeId,int> skillWinningConditions { get; set; }

		public int minScoreWinningCondition { get; set; }

		public bool movesOver {get;set;}
		public IDictionary<ColorId, int> colorSpawnPercentages { get; set; }

		public IDictionary<string, int> itemSpawnPercentages { get; set; }

        public IDictionary<string, int> itemSpawnMin { get; set; }

        public IDictionary<string, int> itemSpawnMax { get; set; }

        public IDictionary<string, int> itemTotalSpawnMin { get; set; }

        public IDictionary<string, int> itemTotalSpawnMax { get; set; }

        public CompanionTypeId companionType { get; set; }

        CompanionTypeId _companionType { get; set; }

        public int companionRequiredCharge { get; set; }

		public bool superMergingEnabled { get; set; }

		public IList<ItemTypeId> preventItems { get; set; }

		public IDictionary<ItemTypeId, int> ensureItems { get; set; }

		public IList<int> starThresholds { get; set; }

		public string bossName { get; set; }

		public int adaptiveColorSpawningOffset { get; set; }// TODO deprecated tbr
		public string spawningSetName { get; set; }

		public int acsMin { get; set; }

		public int acsMax { get; set; }

		public List<ColorId> colorSpawningOrder { get; set; }

		public bool hasBoss
		{
			get
			{
				return !string.IsNullOrEmpty(bossName) && bossName != "0";
			}
		}

		
		Dictionary<string, Dictionary<string, LevelMatrix>> _layers = new Dictionary<string, Dictionary<string, LevelMatrix>>();

        public LevelData()
		{

			id = "anyId";
			name = "new";
			typeWinningConditions = new Dictionary<ItemTypeId, int>();
			colorWinningConditions = new List<ColorWinningCondition>();
			skillWinningConditions = new Dictionary<CompanionTypeId, int> ();
			colorSpawnPercentages = new Dictionary<ColorId, int>
			{
				{ ColorId.Color0, COLOR_CHANCE },
				{ ColorId.Color1, COLOR_CHANCE },
				{ ColorId.Color2, COLOR_CHANCE },
				{ ColorId.Color3, COLOR_CHANCE },
				{ ColorId.Color4, COLOR_CHANCE },
				{ ColorId.Color5, COLOR_CHANCE },
			};

			itemSpawnPercentages = new Dictionary<string, int>
			{
				{ ItemTypeId.Dirt + "_1", 0 },
				{ ItemTypeId.Dirt + "_2", 0 },
				{ ItemTypeId.Dirt + "_3", 0 },
				{ ItemTypeId.Dirt + "_4", 0 },
				{ ItemTypeId.Dirt + "_5", 0 },
				{ ItemTypeId.CannonGem.ToString(), 0 },
				{ ItemTypeId.Question.ToString(), 0 },
			};

            int base_max = 100;
            int base_min = 0;
            int intMax = 10000;
            itemSpawnMax = new Dictionary<string, int>
            {
                { ItemTypeId.Dirt + "_1", base_max },
                { ItemTypeId.Dirt + "_2", base_max },
                { ItemTypeId.Dirt + "_3", base_max },
                { ItemTypeId.Dirt + "_4", base_max },
                { ItemTypeId.Dirt + "_5", base_max },
                { ItemTypeId.CannonGem.ToString(), base_max },
                { ItemTypeId.Question.ToString(), base_max },
                { ItemTypeId.Key +  "_0" , base_max },
                { ItemTypeId.Key +  "_1" , base_max },
                { ItemTypeId.Key +  "_2" , base_max },
                { ItemTypeId.Key +  "_3" , base_max },
                { ItemTypeId.Key +  "_4" , base_max },
                { ItemTypeId.Key +  "_5" , base_max },
            };

            itemSpawnMin = new Dictionary<string,int>
            {
                { ItemTypeId.Dirt + "_1", base_min },
                { ItemTypeId.Dirt + "_2", base_min },
                { ItemTypeId.Dirt + "_3", base_min },
                { ItemTypeId.Dirt + "_4", base_min },
                { ItemTypeId.Dirt + "_5", base_min },
                { ItemTypeId.CannonGem.ToString(), base_min },
                { ItemTypeId.Question.ToString(), base_min },
                { ItemTypeId.Key +  "_0", base_min },
                { ItemTypeId.Key +  "_1", base_min },
                { ItemTypeId.Key +  "_2", base_min },
                { ItemTypeId.Key +  "_3", base_min },
                { ItemTypeId.Key +  "_4", base_min },
                { ItemTypeId.Key +  "_5", base_min },
            };

            itemTotalSpawnMax = new Dictionary<string, int>
            {
                { ItemTypeId.Key.ToString(), base_max }
            };

            itemTotalSpawnMin = new Dictionary<string, int>
            {
                { ItemTypeId.Key.ToString(), base_min }
            };

            preventItems = new List<ItemTypeId>();
			ensureItems = new Dictionary<ItemTypeId, int>();
			minScoreWinningCondition = 0;
			movesOver = false;
			width = LevelEditorInfo.Instance.BoardWidth;
			height = LevelEditorInfo.Instance.BoardHeight;
			superMergingEnabled = true;
			companionRequiredCharge = 50;
			maxMoves = 60;
			starThresholds = new List<int>();

			_layers = new Dictionary<string, Dictionary<string, LevelMatrix>>();
			var fields = _layers[ConfigLayerId.FIELDS] = new Dictionary<string, LevelMatrix>();
			var items = _layers[ConfigLayerId.ITEMS] = new Dictionary<string, LevelMatrix>();
			var covers = _layers[ConfigLayerId.COVERS] = new Dictionary<string, LevelMatrix>();
			fields[ConfigLayerId.TYPES] = new LevelMatrix(width, height, ConfigFieldType.NORMAL[0]);
			fields[ConfigLayerId.HITPOINTS] = new LevelMatrix(width, height);
			items[ConfigLayerId.TYPES] = new LevelMatrix(width, height, '*');
			items[ConfigLayerId.HITPOINTS] = new LevelMatrix(width, height, '1');
			items[ConfigLayerId.COLORS] = new LevelMatrix(width, height, '?');
			covers[ConfigLayerId.TYPES] = new LevelMatrix(width, height);
			covers[ConfigLayerId.HITPOINTS] = new LevelMatrix(width, height);
			covers[ConfigLayerId.OPTIONS] = new LevelMatrix(width, height);

            bossName = "";
            spawningSetName = (LevelEditorInfo.Instance.gameConfig.availableSpawningSets.Count > 0) 
                                ? LevelEditorInfo.Instance.gameConfig.availableSpawningSets[0] 
                                : "";

			for (int x = 0; x < width; x++)
			{
				fields[ConfigLayerId.TYPES].Set(x, 0, ConfigFieldType.SPAWNER);
				items[ConfigLayerId.TYPES].Set(x, 0, ConfigItemType.EMPTY);
				items[ConfigLayerId.HITPOINTS].Set(x, 0, '-');
			}

			adaptiveColorSpawningOffset = 0;

			acsMin = 0;
			acsMax = 0;

			colorSpawningOrder = new List<ColorId>();
		}

		public void Update(JSONNode data)
		{
			id = data.GetString(ID);
			name = data.GetString(NAME);
			levelNum = data.GetInt (LEVEL_NUM);
            maxMoves = data.GetInt(MAX_MOVES, (int)LevelEditorInfo.Instance.DefaultValues[MAX_MOVES]);
            superMergingEnabled = data.GetBoolean(SUPER_MERGING_ENABLED, (bool)LevelEditorInfo.Instance.DefaultValues[SUPER_MERGING_ENABLED]);

			width = data.GetInt(WIDTH, LevelEditorInfo.Instance.BoardWidth);
			height = data.GetInt(HEIGHT, LevelEditorInfo.Instance.BoardHeight);

			ParseStars(data.GetCollection(STARS));

            bossName = data.GetString(BOSS_NAME, (string)LevelEditorInfo.Instance.DefaultValues[BOSS_NAME]);
			string setName = data.GetString(SPAWNING_SET_NAME);
            spawningSetName = (string.IsNullOrEmpty(setName) || setName == "0") 
                                ? spawningSetName 
                                : setName;
			adaptiveColorSpawningOffset = data.GetInt(ADAPTIVE_COLOR_SPAWNING_OFFSET, 0);

            acsMin = data.GetInt(ACS_MIN, (int)LevelEditorInfo.Instance.DefaultValues[ACS_MIN]);
            acsMax = data.GetInt(ACS_MAX, (int)LevelEditorInfo.Instance.DefaultValues[ACS_MAX]);

			ParseLayers(data.GetDictionary(LAYERS));
			ParseConditions(data.GetDictionary(CONDITIONS));
			ParseCompanion(data.GetDictionary(COMPANION));
			ParseColorSpawnPercentages(data);
			ParseItemSpawnPercentages(data);
			ParsePreventCreation(data);
			ParseEnsureItems(data);
			ParseColorSpawningOrder(data);
		}

		public IDictionary<string,object> Serialize()
		{
			var companion = SerializeCompanion();
			var prevents = SerializePrevents();
			var ensures = SerializeEnsures();
			var colorChances = SerializeColorChances();
			var itemChances = SerializeItemChances();
            var minItem = SerializeItemMin();
            var maxItem = SerializeItemMax();
            var totalMinItem = SerializeItemTotalMin();
            var totalMaxItem = SerializeItemTotalMax();
			var conditions = SerializeConditions();
			var layers = SerializeLayers();
			var colorOrder = SerializeColorSpawningOrder();

			var dict = new Dictionary<string, object>
			{
				{ ID, id },
				{ NAME, name },
				{LEVEL_NUM,levelNum},
				{ MAX_MOVES, maxMoves },
				{ SUPER_MERGING_ENABLED, superMergingEnabled },
				{ COMPANION, companion },
				{ PREVENT_ITEMS, prevents },
				{ ENSURE_ITEMS, ensures },
				{ COLOR_SPAWN_PERCENTAGES, colorChances },
				{ ITEM_SPAWN_PERCENTAGES, itemChances },
                { ITEM_SPAWN_MAX,maxItem },
                { ITEM_SPAWN_MIN,minItem },
                { ITEM_TOTAL_SPAWN_MAX, totalMaxItem},
                { ITEM_TOTAL_SPAWN_MIN, totalMinItem},
				{ CONDITIONS, conditions },
				{ LAYERS, layers },
				{ STARS, starThresholds },
				{ BOSS_NAME, bossName },
				{ ACS_MIN, acsMin },
				{ ACS_MAX, acsMax },
				{ SPAWNING_SET_NAME, spawningSetName },
				{ ADAPTIVE_COLOR_SPAWNING_OFFSET, adaptiveColorSpawningOffset },
				{ COLOR_SPAWNING_ORDER, colorOrder },
			};

//            foreach (var pair in LevelEditorInfo.Instance.DefaultValues)
//            {
//                var currentValue = dict[pair.Key];
//                var defaultValue = pair.Value;
//
//                if (object.Equals(currentValue, defaultValue))
//                {
//                    dict.Remove(pair.Key);
//                }
//            }

			return dict;
		}

		Dictionary<string,Dictionary<string,List<string>>> SerializeLayers()
		{
			var layers = new Dictionary<string,Dictionary<string,List<string>>>();

			foreach (var layerPair in _layers)
			{
                var types = layers[layerPair.Key] = new Dictionary<string,List<string>>();

                foreach (var typePair in layerPair.Value)
                {
//                    LevelMatrix defaultMatrix = null;
//
//                    var defaultLayer = new Dictionary<string, LevelMatrix>();
//                    LevelDefault.Layers.TryGetValue(layerPair.Key, out defaultLayer);
//
//                    if (defaultLayer != null)
//                    {
//                        defaultLayer.TryGetValue(typePair.Key, out defaultMatrix);
//                    }
//
//					if (defaultMatrix == null || !defaultMatrix.Equals(typePair.Value)))
//                    {
                        types[typePair.Key] = typePair.Value.Serialize();
//                    }
                }
			}

			return layers;
		}

		Dictionary<string, object> SerializeCompanion()
		{
			var comp = new Dictionary<string, object>();
			comp[TYPE] = _companionType.ToString();
			comp[REQUIRED_CHARGE] = companionRequiredCharge;

			return comp;
		}

		List<string> SerializeColorSpawningOrder()
		{
			var colorOrder = new List<string>();

			foreach (ColorId color in colorSpawningOrder)
			{
				colorOrder.Add(color.ToString());
			}

			return colorOrder;
		}

		List<string> SerializePrevents()
		{
			var prevents = new List<string>();

			foreach (var preventItem in preventItems)
			{
				prevents.Add(preventItem.ToString());
			}

			return prevents;
		}

		Dictionary<string, int> SerializeEnsures()
		{
			var ensures = new Dictionary<string, int>();

			foreach (var pair in ensureItems)
			{
				var key = pair.Key.ToString().LowercaseFirst();
				ensures[key] = pair.Value;
			}

			return ensures;
		}

		Dictionary<string, int> SerializeColorChances()
		{
			var chances = new Dictionary<string, int>();

			foreach (var pair in colorSpawnPercentages)
			{
				var key = pair.Key.ToString().LowercaseFirst();
				chances[key] = pair.Value;
			}

			return chances;
		}

        Dictionary<string,int> SerializeItemMax()
        {
            var maxItem = new Dictionary<string, int>();
            foreach (var pair in itemSpawnMax)
            {
                maxItem[pair.Key] = pair.Value;
            }
            return maxItem;
        }

        Dictionary<string,int> SerializeItemMin()
        {
            var minItem = new Dictionary<string, int>();
            foreach (var pair in itemSpawnMin)
            {
                minItem[pair.Key] = pair.Value;
            }
            return minItem;
        }

        Dictionary<string, int> SerializeItemTotalMax()
        {
            var maxItem = new Dictionary<string, int>();
            foreach (var pair in itemTotalSpawnMax)
            {
                maxItem[pair.Key] = pair.Value;
            }
            return maxItem;
        }

        Dictionary<string,int> SerializeItemTotalMin()
        {
            var minItem = new Dictionary<string, int>();
            foreach (var pair in itemTotalSpawnMin)
            {
                minItem[pair.Key] = pair.Value;
            }
            return minItem;
        }

        Dictionary<string, int> SerializeItemChances()
		{
			var chances = new Dictionary<string, int>();

			foreach (var pair in itemSpawnPercentages)
			{
				chances[pair.Key] = pair.Value;
			}

			return chances;
		}

		Dictionary<string, int> SerializeConditions()
		{
			var conditions = new Dictionary<string, int>();

			if (minScoreWinningCondition > 0) {
			conditions [SCORE] = minScoreWinningCondition;
			}

			conditions[MOVESOVER] = movesOver?1:0;

			foreach (var pair in typeWinningConditions)
			{
				var key = pair.Key.ToString().ToLower();
				conditions[key] = pair.Value;
			}

			foreach (var cond in colorWinningConditions)
			{
				var colors = cond.colors;
				var key = colors[0].ToString();

				for (int i = 1; i < colors.Count; i++)
				{
					key += "," + colors[i];
				}

				conditions[key] = cond.amount;
			}

			foreach (var pair in skillWinningConditions) {
				var key = pair.Key.ToString();
				if(pair.Value>0)
				{
					conditions[key] = pair.Value;
				}
			}

			return conditions;
		}

		void ParseLayers(JSONObject dict)
		{
			foreach (var layerKey in dict.Keys)
			{
				var layerData = ((JSONNode) dict).GetDictionary(layerKey);
				var layer = _layers[layerKey];

				foreach (var typeKey in layerData.Keys)
				{
					var typeData = layerData[typeKey].AsCollection();

                    if (typeKey == "connections" || typeKey == ConfigLayerId.OPTIONS)
                    {
                        var matrix = new LevelMatrix(typeData, width, height);
                        if (layer.ContainsKey(ConfigLayerId.OPTIONS))
                        {
                            for (int x = 0; x < matrix.width; x++)
                            {
                                for (int y = 0; y < matrix.height; y++)
                                {
                                    if (layer[ConfigLayerId.OPTIONS].Get(x, y) != '-')
                                    {
                                        if(matrix.Get(x, y) == '-')
                                        {
                                            matrix.Set(x, y, layer[ConfigLayerId.OPTIONS].Get(x, y));
                                        }
                                        else
                                        {
                                            Debug.LogError("MERGE CONFLICT Cannot merge level " + id + ", " + name);
                                        }
                                    }
                                }
                            }
                        }

                        layer[ConfigLayerId.OPTIONS] = matrix;
                    }
                    else
                    {
                        layer[typeKey] = new LevelMatrix(typeData, width, height);
                    }
					
				}
			}
		}

		void ParseStars(IList<JSONNode> list)
		{
			starThresholds.Clear();

			if (list != null)
			{
				for (int i = 0, listCount = list.Count; i < listCount; i++)
				{
					var item = list[i];
					starThresholds.Add(item.AsInt());
				}
			}
		}

		void ParseConditions(JSONObject conditions)
		{
			typeWinningConditions.Clear();
			colorWinningConditions.Clear();
			skillWinningConditions.Clear ();
			var keys = conditions.Keys;

			foreach (string key in keys)
			{
				if (key == SCORE)
				{
					minScoreWinningCondition = (int) conditions[key];
				}
				else if(key == MOVESOVER)
				{
					movesOver = (int)conditions[key]>0?true:false;
				}
				else
				{
					try
					{
						ItemTypeId typeId = LevelEditorUtils.ParseEnum<ItemTypeId>(key.UppercaseFirst());
						int amount = (int) conditions[key];
						typeWinningConditions.Add(typeId, amount);
					}
					catch
					{
						try
						{
							string[] colorsStrings = key.Split(',');

							var colorIds = new List<ColorId>();

							foreach (string colorString in colorsStrings)
							{
								ColorId colorId = LevelEditorUtils.ParseEnum<ColorId>(colorString.UppercaseFirst());
								colorIds.Add(colorId);
							}

							int amount = (int) conditions[key];

							var colorWinningCondition = new ColorWinningCondition();
							colorWinningCondition.amount = amount;
							colorWinningCondition.colors = colorIds;

							colorWinningConditions.Add(colorWinningCondition);
						}
						catch
						{
							try
							{
								CompanionTypeId skillType = LevelEditorUtils.ParseEnum<CompanionTypeId>(key);
								int amount = (int) conditions[key];
								skillWinningConditions.Add(skillType,amount);
							}
							catch
							{
								Debug.LogError("Could not parse winning condition for key = " + key + ", name = " + name);
							}
						}
					}
				}
			}
		}

		void ParseCompanion(JSONObject companion)
		{
			if (companion.Count == 0
			    || !companion.ContainsKey(TYPE))
			{
				companionType = CompanionTypeId.Empty;
                companionRequiredCharge = 0;
			}
			else
			{
				companionType = LevelEditorUtils.ParseEnum<CompanionTypeId>(companion[TYPE].AsString());
				companionRequiredCharge = (int) companion[REQUIRED_CHARGE];
			}
            _companionType = companionType;
		}

		void ParseEnsureItems(JSONNode data)
		{
			ensureItems.Clear();

			if (data.HasKey(ENSURE_ITEMS))
			{
				var dict = data.GetDictionary(ENSURE_ITEMS);

				if (dict != null
					&& dict.Count > 0)
				{
					try
					{				
						foreach (var key in dict.Keys)
						{
							int amount = (int) dict[key];
							var itemType = LevelEditorUtils.ParseEnum<ItemTypeId>(key.UppercaseFirst());
							ensureItems[itemType] = amount;
						}
					}
					catch (Exception ex)
					{
						ensureItems.Clear();
						Debug.Log("LEVEL : ensureItems = " + JSON.Serialize(dict));
						Debug.LogError("LEVEL : FAILED TO PARSE ensureItems"+ ex.Message);
					}
				}
			}
		}

		void ParseColorSpawnPercentages(JSONNode data)
		{
			var dict = data.GetDictionary(COLOR_SPAWN_PERCENTAGES);
			colorSpawnPercentages.Clear();

			if (dict.Count > 0)
			{
				try
				{				
					foreach (var key in dict.Keys)
					{
						int amount = (int) dict[key];
						var colorId = LevelEditorUtils.ParseEnum<ColorId>(key.UppercaseFirst());
						colorSpawnPercentages[colorId] = amount;
					}
				}
				catch (Exception ex)
				{
					colorSpawnPercentages.Clear();
					Debug.Log("LEVEL : colorSpawnPercentages = " + JSON.Serialize(dict));
					Debug.LogError("Failed to parse colorSpawnPercentages"+ ex.Message);
				}
			}
			else
			{
				foreach (var color in ColorUtils.allColors)
				{
					colorSpawnPercentages[color] = COLOR_CHANCE;
				}
			}
		}

		void ParseItemSpawnPercentages(JSONNode data)
		{
			if (data.HasKey(ITEM_SPAWN_PERCENTAGES))
			{
				itemSpawnPercentages.Clear();

				var dict = data.GetDictionary(ITEM_SPAWN_PERCENTAGES);

				foreach (var type in ITEM_TYPES)
				{
					string key = type.ToString();
					itemSpawnPercentages[type] = dict.ContainsKey(key) ? (int) dict[key] : 0;
				}
			}
            if(data.HasKey(ITEM_SPAWN_MAX))
            {
                itemSpawnMax.Clear();
                var dic = data.GetDictionary(ITEM_SPAWN_MAX);
                foreach(var type in ITEM_TYPES)
                {
                    string key = type.ToString();
                    itemSpawnMax[type] = dic.ContainsKey(key) ? (int)dic[key] : 100;
                }
            }
            if(data.HasKey(ITEM_SPAWN_MIN))
            {
                itemSpawnMin.Clear();
                var dic = data.GetDictionary(ITEM_SPAWN_MIN);
                foreach(var type in ITEM_TYPES)
                {
                    string key = type.ToString();
                    itemSpawnMin[type] = dic.ContainsKey(key) ? (int)dic[key] : 0;
                }
            }
            if(data.HasKey(ITEM_TOTAL_SPAWN_MIN))
            {
                itemTotalSpawnMin.Clear();
                var dic = data.GetDictionary(ITEM_TOTAL_SPAWN_MIN);
                foreach(var type in TOTAL_ITEM_TYPES)
                {
                    string key = type.ToString();
                    itemTotalSpawnMin[type] = dic.ContainsKey(key) ? (int)dic[key] : 0;
                }
            }
            if(data.HasKey(ITEM_TOTAL_SPAWN_MAX))
            {
                itemTotalSpawnMax.Clear();
                var dic = data.GetDictionary(ITEM_TOTAL_SPAWN_MAX);
                foreach(var type in TOTAL_ITEM_TYPES)
                {
                    string key = type.ToString();
                    itemTotalSpawnMax[type] = dic.ContainsKey(key) ? (int)dic[key] : 0;
                }
            }
		}

		void ParsePreventCreation(JSONNode data)
		{
			var items = data.GetCollection(PREVENT_ITEMS);

			if (items.Count > 0)
			{
				preventItems.Clear();

				try
				{
					foreach (var item in items)
					{
						ItemTypeId typeId = LevelEditorUtils.ParseEnum<ItemTypeId>(item.AsString().UppercaseFirst());
						preventItems.Add(typeId);
					}
				}
				catch (Exception ex)
				{
					preventItems.Clear();
					Debug.LogError("LEVEL : Failed to parse preventCreation"+ ex.Message);
				}

			}
		}

		void ParseColorSpawningOrder(JSONNode data)
		{
			if (!data.HasKey(COLOR_SPAWNING_ORDER))
			{
				return;
			}

			var items = data.GetCollection(COLOR_SPAWNING_ORDER);

			if (items.Count > 0)
			{
				colorSpawningOrder.Clear();

				try
				{
					foreach (var item in items)
					{
						ColorId colorId = LevelEditorUtils.ParseEnum<ColorId>(item.AsString().UppercaseFirst());
						colorSpawningOrder.Add(colorId);
					}
				}
				catch (Exception ex)
				{
					colorSpawningOrder.Clear();
					Debug.LogError("LEVEL : Failed to parse colorSpawningOrder"+ ex.Message);
				}
			}
		}

		public bool IsObjectiveType(ItemTypeId itemType)
		{
			return typeWinningConditions.ContainsKey(itemType);
		}

		public string GetFromLayer(string layerId, string typeId, int x, int y)
		{
			Dictionary<string, LevelMatrix> layer;
			
			if (_layers.TryGetValue(layerId, out layer))
			{
				LevelMatrix matrix;

				if (layer.TryGetValue(typeId, out matrix))
				{
					return matrix.Get(x, y).ToString();
				}
			}

			if (LevelEditorInfo.Instance.DefaultLayers.TryGetValue(layerId, out layer))
			{
				LevelMatrix matrix;
				
				if (layer.TryGetValue(typeId, out matrix))
                {
                    return matrix.Get(x, y).ToString();
                }
            }

			Debug.LogError("LEVEL : Could not get layer '" + layerId + "' and type '" + typeId + "' in level.");
			return "";
		}

		public void SetToLayer(string layerId, string typeId, int x, int y, string value)
		{
			Dictionary<string, LevelMatrix> layer;
			
			if (_layers.TryGetValue(layerId, out layer))
			{
				LevelMatrix matrix;

				if (layer.TryGetValue(typeId, out matrix))
				{
					matrix.Set(x, y, value);
					return;
				}
			}


			if (LevelEditorInfo.Instance.DefaultLayers.TryGetValue(layerId, out layer))
			{
                if (!_layers.ContainsKey(layerId))
                {
                    _layers.Add(layerId, new Dictionary<string, LevelMatrix>());
                }

				LevelMatrix matrix;
				
				if (layer.TryGetValue(typeId, out matrix))
				{
                    if (!_layers[layerId].ContainsKey(typeId))
                    {
                        _layers[layerId].Add(typeId, new LevelMatrix(matrix));
                    }
					
                    _layers[layerId][typeId].Set(x, y, value);
                    return;
                }
            }

			Debug.LogError("LEVEL : Could not set layer '" + layerId + "' and type '" + typeId + "' in level at " + x + "," + y);
		}

		public LevelMatrix GetMatrix(string layerId, string typeId)
		{
			Dictionary<string, LevelMatrix> layer;
			
			if (_layers.TryGetValue(layerId, out layer))
			{
				LevelMatrix matrix;
				
				if (layer.TryGetValue(typeId, out matrix))
				{
					return matrix;
				}
			}
			
			if (LevelEditorInfo.Instance.DefaultLayers.TryGetValue(layerId, out layer))
			{
				LevelMatrix matrix;
				
				if (layer.TryGetValue(typeId, out matrix))
				{
					return matrix;
                }
            }
            
            Debug.LogError("LEVEL : Could not get layer '" + layerId + "' and type '" + typeId + "' in level.");
			return null;
		}

		public LevelData Clone()
		{
			var data = Serialize();
			var json = JsonMapper.ToJson(data);
			var dict = JSON.Deserialize(json);
            var level = new LevelData();
			level.Update(dict);
			return level;
		}

		public int GetStarsForScore(int score)
		{
			int stars = 0;

			if (starThresholds != null && starThresholds.Count == 3)
			{
				for (int j = starThresholds.Count - 1; j >= 0; j--)
				{
					if (score >= starThresholds[j])
					{
						stars = j + 1;
						break;
					}
				}
			}

			return stars;
		}
	}


}
