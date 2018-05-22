using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{
    public class ComMoveLevelDown : ICommand
    {
        LevelData _selectedLevel;
        LevelDataList _list;
        public ComMoveLevelDown(LevelDataList list, LevelData level)
        {
            _list = list;
            _selectedLevel = level;
        }
        public bool Execute()
        {
            return _list.MoveLevelDown(_selectedLevel);
            
        }

        public void Undo()
        {

            _list.MoveLevelUp(_selectedLevel);
        }
    }
}
