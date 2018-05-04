using System.Collections.Generic;
using Wooga.Foundation.Json;
using System;

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
                    _list[i].levelNum = i;
                }
                else
                {
                    break;
                }
            }
        }
        #region public methods

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