using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEditor;
using System.IO;

namespace CommonLevelEditor
{
    public class LevelListScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<LevelData> _data;
        private LevelDataList _levelList;
        public EnhancedScroller myScroller;
        public LevelCellView levelCellViewPrefab;
        public float cellHeight= 100f;
        void Start()
        {
            LoadLevelList();
            
            


            myScroller.Delegate = this;
            myScroller.ReloadData();
        }

        void LoadLevelList()
        {
            _levelList = new LevelDataList();

            foreach (var typename in LevelEditorInfo.Instance.LevelNumToLevelType.Values)
            {
                var files = Directory.GetFiles(LevelEditorInfo.Instance.FullConfigurationFolderPath, typename + "*.json");
                foreach (var filename in files)
                {

                    var node = LevelEditorUtils.JSONNodeFromFile(filename);

                    _levelList.Update(node);

                }
            }

        }
        public int GetNumberOfCells(EnhancedScroller scroller) 
        {
             return _levelList.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) 
        {

             return cellHeight; 
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            LevelCellView cellView = scroller.GetCellView(levelCellViewPrefab) as LevelCellView;
            cellView.name = _levelList[dataIndex].name;
            cellView.onSelected += CellViewSelected;
            cellView.SetData(dataIndex, _levelList[dataIndex]);

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
                for (var i = 0; i < _levelList.Count; i++)
                {
                    _levelList[i].Selected = (selectedDataIndex == i);
                }
            }
        }
    }
}
