using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLevelEditor
{
    public class UndoRedoList
    {
        private List<ICommand> _list ;
        private int _idx;

        public UndoRedoList ()
        {
            _list = new List<ICommand>();
            _idx = 0;
        }

        public void Undo()
        {
            if (_idx >0)
            {
                _idx--;
                _list[_idx].Undo();
                
            }
        }

        public void Redo()
        {
            if (_idx < _list.Count)
            {
                
                _list[_idx].Execute();
                _idx++;
            }
        }

        public void Add(ICommand command)
        {
            if (_idx < _list.Count)
            {
                _list.RemoveRange(_idx, _list.Count - _idx);
            }
            _list.Add(command);
            _idx++;
        }

        public bool IsEmpty()
        {
            return _list.Count == 0;
        }
    }
}
