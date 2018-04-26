using System;

namespace GeneralLevelEditor
{
    public class LevelData 
    { 
        #region 
            
        #endregion

        public event Action<bool> selectedChanged;

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                // if the value has changed
                if (_selected != value)
                {
                    // update the state and call the selection handler if it exists
                    _selected = value;
                    if (selectedChanged != null) selectedChanged(_selected);
                }
            }
        }

        public string levelName;
    }
}
