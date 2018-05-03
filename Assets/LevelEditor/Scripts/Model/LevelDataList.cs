using System.Collections.Generic;
using Wooga.Foundation.Json;

namespace CommonLevelEditor
{
    public class LevelDataList : IUpdatable
    {
        private const string CONTENT = "content";

        private List<LevelData> _list = new List<LevelData>();

        #region property
        public int Count 
        {
            get{
                return _list.Count;
            }
        }

        public LevelData this[int i]
        {
            get { return _list[i]; }
            set { _list[i] = value; }
        }

        #endregion

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
            _list.Sort((a,b) => a.levelNum.CompareTo(b.levelNum));
        }



        #endregion
    
    }    
}