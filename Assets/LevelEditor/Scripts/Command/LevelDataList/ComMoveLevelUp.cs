using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{
    public class ComMoveLevelUp : ICommand
    {
        LevelData _selectedLevel;
        LevelDataList _list;
        public ComMoveLevelUp(LevelDataList list, LevelData level)
        {
            _list = list;
            _selectedLevel = level;
        }
        public bool Execute()
        {
            
            return _list.MoveLevelUp(_selectedLevel);

        }

        public void Undo()
        {
            
            _list.MoveLevelDown(_selectedLevel);
            
        }
    }
}
