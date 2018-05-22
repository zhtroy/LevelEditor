using System.Collections.Generic;
using Wooga.Foundation.Json;
using System;
using LitJson;
using System.IO;

namespace CommonLevelEditor
{
    public class LevelDataList : IUpdatable
    {
        private const string CONTENT = "content";

        private List<LevelData> _list = new List<LevelData>();

        public event Action onDataChange;
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
            if (onDataChange!=null)
            {
                onDataChange();
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
                            if (levelTypeNames[i].EndsWith("*"))
                            {
                                SaveLevels(path + levelTypeNames[i].TrimEnd(new char[]{'*'}) + "_" + levelFileNum.ToString(), tempList);
                                levelFileNum++;

                                tempList.Clear();
                            }
                            
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
                    if (levelTypeNames[i].EndsWith("*"))
                    {
                        SaveLevels(path + levelTypeNames[i].TrimEnd(new char[] { '*' }) + "_" + levelFileNum.ToString(), tempList);
                    }
                    else
                    {
                        SaveLevels(path + levelTypeNames[i], tempList);
                    }
                    
                    
                }
                if (tempList.Count==0)
                {
                    //删除不用的文件
                    if (!levelTypeNames[i].EndsWith("*"))
                    {
                        var files = Directory.GetFiles(LevelEditorInfo.Instance.FullConfigurationFolderPath, levelTypeNames[i] + ".json");
                        foreach (var filename in files)
                        {
                            File.Delete(filename);
                                
                        }
                    }
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

                //有相同levelNum的就不加了
                if (_list.Find(x=>x.levelNum == levelData.levelNum)!=null)
                {
                    continue;
                }

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
            if (onDataChange != null)
            {
                onDataChange();
            }
        }

        public void AddLevel(LevelData level)
        {
            _list.Add(level);
            SortLevelList();
            if (onDataChange != null)
            {
                onDataChange();
            }
        }

        public bool MoveLevelUp(LevelData level)
        {
            int idx = _list.IndexOf(level);
            if (idx<=0)
            {
                return false;
            }

            SwapLevelNum(level, _list[idx - 1]);
            SortLevelList();

            return true;
        }

        public bool MoveLevelDown(LevelData level  )
        {
            int idx = _list.IndexOf(level);
            if (idx<0)
            {
                return false;
            }
            if (idx >= _list.Count-1)
            {
                return false;
            }
            SwapLevelNum(level, _list[idx + 1]);
            SortLevelList();
            return true;

        }

        void SwapLevelNum(LevelData a, LevelData b)
        {
            int tempNum = a.levelNum;
            a.levelNum = b.levelNum;
            b.levelNum = tempNum;
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