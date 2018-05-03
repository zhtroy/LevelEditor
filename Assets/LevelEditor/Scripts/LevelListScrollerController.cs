using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

namespace CommonLevelEditor
{
    public class LevelListScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<LevelData> _data;
        public EnhancedScroller myScroller;
        public LevelCellView levelCellViewPrefab;
        public float cellHeight= 100f;
        void Start()
        {
            _data = new List<LevelData>();

            for (int i = 0; i < 1000; i++)
            {
                _data.Add(new LevelData() { name = "Mouse" });
            }
            myScroller.Delegate = this;
            myScroller.ReloadData();
        }
        public int GetNumberOfCells(EnhancedScroller scroller) 
        {
             return _data.Count; 
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) 
        {

             return cellHeight; 
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            LevelCellView cellView = scroller.GetCellView(levelCellViewPrefab) as LevelCellView;
            cellView.name = _data[dataIndex].name;
            cellView.onSelected += CellViewSelected;
            cellView.SetData(dataIndex, _data[dataIndex]);

            return cellView;
        }


        private void CellViewSelected(EnhancedScrollerCellView cellView)
        {
            if (cellView == null)
            {
               
            }
            else
            {
                // get the selected data index of the cell view
                var selectedDataIndex = (cellView as LevelCellView).DataIndex;

                // loop through each item in the data list and turn
                // on or off the selection state. This is done so that
                // any previous selection states are removed and new
                // ones are added.
                for (var i = 0; i < _data.Count; i++)
                {
                    _data[i].Selected = (selectedDataIndex == i);
                }
            }
        }
    }
}
