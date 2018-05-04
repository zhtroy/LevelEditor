using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{
    public class ComDeleteLevel : ICommand
    {
        LevelData _deletedLevel;
        LevelDataList _list;
        int _deletedIdx;
        public ComDeleteLevel(LevelDataList list)
        {
            _list = list;
        }
        public void Execute()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Selected)
                {
                    _deletedLevel = _list[i];
                    _deletedIdx = i;
                    _list.DeleteLevel(i);
                    break;   //delete the first selected level
                }
            }

        }

        public void Undo()
        {
            
            _list.InsertLevel(_deletedIdx, _deletedLevel);
            _deletedLevel.Selected = false;
        }
    }
}
