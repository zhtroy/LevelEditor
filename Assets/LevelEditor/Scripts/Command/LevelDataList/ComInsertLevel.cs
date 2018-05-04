using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{
    public class ComAddLevel : ICommand
    {
        LevelData _addedLevel;
        LevelDataList _list;
        public ComAddLevel(LevelDataList list, LevelData level)
        {
            _list = list;
            _addedLevel = level;
        }
        public void Execute()
        {
            _list.InsertLevel(_addedLevel);
            _list.SelectSingleLevel(_addedLevel);
        }

        public void Undo()
        {
            
            _list.DeleteLevel( _addedLevel);
        }
    }
}
