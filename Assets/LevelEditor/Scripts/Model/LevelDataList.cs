using System.Collections.Generic;
using Wooga.Foundation.Json;
using System;
using LitJson;

namespace CommonLevelEditor
{
    public class LevelDataList : IUpdatable
    {
        private const string CONTENT = "content";

        private List<LevelData> _list = new List<LevelData>();

        public event Action OnDataChange;
        #region property
        public int Count 
        {
            get{
                return _list.Count;
            }
        }

        public LevelData CurrentSelectedLevel { get; private set; }
        public LevelData this[int i]
        {
            get { return _list[i]; }
            set { _list[i] = value; }
        }

        #endregion


        void SortLevelList()
        {
            _list.Sort((a, b) => a.levelNum.CompareTo(b.levelNum));
            for(int i=0; i<_list.Count;i++)
            {
                if (_list[i].levelNum<= LevelEditorInfo.Instance.SortLevelBeforeThisNum)
                {
                    _list[i].levelNum = i + 1;
                }
                else
                {
                    break;
                }
            }
        }

        #region private save level
        void SaveLevels(string path, List<LevelData> list)
        {
            IDictionary<string, object> dic = Serialize(list);
            SaveFile(path + ".json", dic);
        }

        void SaveFile(string path, IDictionary<string, object> dict)
        {
            string json = JsonMapper.ToJson(dict);
            json = JsonFormatter.FormatJson(json);
            System.IO.File.WriteAllText(path, json);
        }


        IDictionary<string, object> Serialize(List<LevelData> unit_levels)
        {
            var dict = new Dictionary<string, object>();
            dict[CONTENT] = GetLevelData(unit_levels);
            return dict;
        }

        IDictionary<string, object> GetLevelData(List<LevelData> unit_levels)
        {
            var dict = new SortedDictionary<string, object>();

            for (int i = 0, _levelsCount = unit_levels.Count; i < _levelsCount; i++)
            {
                var level = unit_levels[i];
                int levelNum = level.levelNum;
                var levelData = level.Serialize();
                dict[levelNum.ToString("0000")] = levelData;
            }
            return dict;
        }
        #endregion

        #region public methods

        public void SaveLevelsToPath(string path)
        {
            
            List<int> levelNumRanges = new List<int>();
            List<string> levelTypeNames = new List<string>();

            foreach (var pair in LevelEditorInfo.Instance.LevelNumToLevelType)
            {
                levelNumRanges.Add(pair.Key);
                levelTypeNames.Add(pair.Value);
            }

            int idx = 0;
            int listNum = 0;
            int levelFileNum;

            for (int i = 0  ; i < levelNumRanges.Count; i++)
            {
                List<LevelData> tempList = new List<LevelData>();
                levelFileNum = 1;

                while (idx<this.Count)
                {
                    var level = this[idx];
                    
                    
                    if (level.levelNum <= levelNumRanges[i])
                    {
                        tempList.Add(level);
                        
                        if (tempList.Count>= LevelEditorInfo.Instance.LevelsPerFile)
                        {
                            SaveLevels(path + levelTypeNames[i] + "_" + levelFileNum.ToString(), tempList);
                            levelFileNum++;
                         
                            tempList.Clear();
                        }
                        idx++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (tempList.Count > 0)
                {
                    SaveLevels(path + levelTypeNames[i] + "_" + levelFileNum.ToString(), tempList);
                    
                }
                
            }
        }


        public void Update(JSONNode data)
        {
            var content = data.GetDictionary(CONTENT);

            foreach (var key in content.Keys)
            {
                JSONNode levelNode = content[key];
                LevelData levelData = new LevelData();
                levelData.Update(levelNode);
                _list.Add(levelData);
            }
            SortLevelList();
        }

        public void DeleteLevel(LevelData level)
        {
            _list.Remove(level);
            SortLevelList();
            if (level.Selected)
            {
                CurrentSelectedLevel = null;
            }
            if (OnDataChange != null)
            {
                OnDataChange();
            }
        }

        public void AddLevel(LevelData level)
        {
            _list.Add(level);
            SortLevelList();
            if (OnDataChange != null)
            {
                OnDataChange();
            }
        }

        public int IndexFromLevelId(int levelId)
        {
            int idx =  _list.FindIndex(item => item.levelNum == levelId);
            return idx;

        }
        public LevelData DataFromLevelId(int levelId)
        {
            int idx = IndexFromLevelId(levelId);
            if (idx == -1)
            {
                return null;
            }
            return this[idx];
        }


        public void SelectSingleLevel(LevelData level)
        {
            foreach (var item in _list)
            {
                if (level == item)
                {
                    item.Selected = true;
                    CurrentSelectedLevel = item;
                }
                else
                {
                    item.Selected = false;
                }
            }
        }


        #endregion public 

        


    }    
}