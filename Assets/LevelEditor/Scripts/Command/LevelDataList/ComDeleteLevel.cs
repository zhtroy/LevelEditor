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
        public ComDeleteLevel(LevelDataList list, LevelData level)
        {
            _list = list;
            _deletedLevel = level;
        }
        public void Execute()
        {
       
            _list.DeleteLevel(_deletedLevel);
        

        }

        public void Undo()
        {
            
            _list.AddLevel( _deletedLevel);
            _list.SelectSingleLevel(_deletedLevel);
        }
    }
}
