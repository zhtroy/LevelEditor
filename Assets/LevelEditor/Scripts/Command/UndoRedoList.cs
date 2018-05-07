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
        private int _saveIdx;

        public event Action OnDirty;
        public event Action OnClean;

        public void Save()
        {
            _saveIdx = _idx;
            CheckDirtyClean();

        }

        private void CheckDirtyClean()
        {
            if (_idx == _saveIdx)
            {
                if (OnClean != null)
                {
                    OnClean();
                }
            }
            else
            {
                if (OnDirty!= null)
                {
                    OnDirty();
                }
            }
        }
        public UndoRedoList ()
        {
            _list = new List<ICommand>();
            _idx = 0;
            _saveIdx = 0;
        }

        public void Undo()
        {
            if (_idx >0)
            {
                _idx--;
                _list[_idx].Undo();
                CheckDirtyClean();
            }
        }

        public void Redo()
        {
            if (_idx < _list.Count)
            {
                
                _list[_idx].Execute();
                _idx++;
                CheckDirtyClean();
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
            CheckDirtyClean();
        }

        public bool IsEmpty()
        {
            return _list.Count == 0;
        }
    }
}
