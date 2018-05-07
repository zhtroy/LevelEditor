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

        #region public events
        public event Action<string, int> onDataChanged;
        #endregion
        #region property
        public int CellNum { get; private set; }
        #endregion
        #region public function
        public EditorBoard(int w, int h)
        {
            _width = w;
            _height = h;
            CellNum = _width * _height;
            
        }
        public void SetItemAt(string layername, int index, string item)
        {
            if (_layers.ContainsKey(layername))
            {
                _layers[layername][index] = item;
                if (onDataChanged!=null)
                {

                    onDataChanged(layername, index);
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
