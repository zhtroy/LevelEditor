using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{ 
    public class EditorBoard
    {
        Dictionary<string, List<string>> _layers = new Dictionary<string, List<string>>();
        int _width;
        int _height;
        LevelData _levelData;

        #region public events
        //params: layername,  index, item name
        public event Action<string, int,string> onDataChanged;
        #endregion
        #region property
        public int CellNum { get; private set; }

        public int Width
        {
            get
            {
                return _width;
            }
        }
        public int Height
        {
            get
            {
                return _height;
            }
        }
        public Dictionary<string,List<string>> Layers
        {
            get
            {
                return _layers;
            }
        }
        #endregion
        #region public function

        
        public EditorBoard(int w, int h,LevelData leveldata)
        {
            _width = w;
            _height = h;
            CellNum = _width * _height;
            _levelData = leveldata;
            UpdateFromLevelData(_levelData);


        }
        

        public string SetItemAt(string layername, int gridX, int gridY, string item )
        {
            int idx = gridX + gridY * _width;
            return SetItemAt(layername, idx, item);
        }
        //if success ,return old item
        //if fail, return null
        public string SetItemAt(string layername, int index, string item)
        {
            if (_layers.ContainsKey(layername))
            {
                string oldItem = _layers[layername][index];
                if (_layers[layername][index]  != item)
                {
                    _layers[layername][index] = item;
                    if (onDataChanged != null)
                    {

                        onDataChanged(layername, index, item);
                    }
                }
                return oldItem;
            }
            return null;
        }

        //从LevelData中解析识别出BoardItem
        void UpdateFromLevelData(LevelData leveldata)
        {
            //use fixed for now
            List<string> layerList = new List<string> { ConfigLayerId.FIELDS, ConfigLayerId.ITEMS, ConfigLayerId.COVERS };
           
            foreach (var name in layerList)
            {
                _layers.Add(name, new List<string>());
            }
            foreach (string  layername in layerList)
            {
                for (int y = 0; y < _height; y++)
                    for (int x = 0; x < _width; x++)
                    {
                    
                        {
                        bool match = false;
                        foreach (var item in LevelEditorInfo.Instance.DicBoardItem.Values)
                        {
                            if (item.LayerId !=layername)
                            {
                                continue;
                            }
                            int count = item.SubLayerChars.Count;
                            foreach (var pair in item.SubLayerChars)
                            {

                                string word = leveldata.GetFromLayer(layername, pair.Key, x, y);
                                if (pair.Value == word)
                                {
                                    count--;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (count <=0) //match
                            {
                                match = true;
                                _layers[layername].Add(item.Name);
                                break;
                            }

                        }
                        if (!match)
                        {
                            _layers[layername].Add(null);
                        }
                       
                    }
                }
            }
        }

        public string GetItemAt(string layername, int index)
        {
            string ret = null;
            if (_layers.ContainsKey(layername))
            {
                ret = _layers[layername][index];
            }
            return ret;
        }

 
        #endregion
    }
}
