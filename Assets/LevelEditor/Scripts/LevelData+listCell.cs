using System;
using Wooga.Foundation.Json;

namespace GeneralLevelEditor
{
    // 关卡列表cell需要的
    public partial class LevelData 
    { 

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
    }
}
